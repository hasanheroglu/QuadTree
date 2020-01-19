using Manager;
using Tree;
using UnityEngine;

namespace Entity
{ 
    public abstract class Entity : MonoBehaviour
    {
        public float PosX { get; set; }
        public float PosY { get; set; }
        public int Health { get; set; }
        public bool IsDestroyed { get; set; }
        public QuadTree ParentQuadTree { get; set; }
        
        private void Awake()
        {
            Health = 5;
            IsDestroyed = false;
        }
        private void Update()
        {
            if (gameObject.transform.localScale == Vector3.zero && IsDestroyed)
            {
                ParentQuadTree.Entities.Remove(this);
                Destroy(gameObject);
            }
            
            if (Health <= 0  && !IsDestroyed)
            {
                StartCoroutine(Tween.Instance.Shrink(gameObject, gameObject.transform.localScale, Vector3.zero, 0.1f));
                IsDestroyed = true;
                TestQuadTree.Instance.RemoveEntity();
            }
        }

        public abstract void SetPosition(Vector3 position);
        public abstract void SetSize(float width, float height);
        public abstract bool Intersects(Tree.Rectangle rectangle);
        public abstract void FindCollisions(QuadTree quadTree);

        protected void Collide()
        {
            Health--;
            gameObject.GetComponent<ParticleSystem>().Play();
            AudioManager.Instance.PlayCollision();
        }
    }
}
