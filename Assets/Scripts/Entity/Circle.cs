using System.Collections.Generic;
using Manager;
using Tree;
using UnityEngine;

namespace Entity
{
    public class Circle : Entity
    {
        public float Radius { get; set; }

        public override void SetPosition(Vector3 position)
        {
            gameObject.transform.SetPositionAndRotation(position, Quaternion.identity);
            PosX = position.x;
            PosY = position.y;        }

        public override void SetSize(float width, float height)
        {
            Radius = width/2;
            StartCoroutine(Tween.Instance.Grow(gameObject, Vector3.zero, new Vector3(width, width, width), 0.1f));
        }

        public override bool Intersects(Tree.Rectangle rectangle)
        {
            var distanceX = Mathf.Abs(PosX - rectangle.PosX);
            var distanceY = Mathf.Abs(PosY - rectangle.PosY);
            var cornerDistance = Mathf.Pow(distanceX - rectangle.Width, 2) + Mathf.Pow(distanceY - rectangle.Height, 2);

            if (PosX < rectangle.PosX + rectangle.Width && 
                PosX > rectangle.PosX - rectangle.Width &&
                PosY < rectangle.PosY + rectangle.Height &&
                PosY > rectangle.PosY - rectangle.Height) { //inside the rectangle
                return true;
            }

            if (distanceX > Radius + rectangle.Width || distanceY > Radius + rectangle.Height)
            {
                return false; 
            }

            if (distanceX <= Radius + rectangle.Width)
            {
                return true;
            }

            if (distanceY <= Radius + rectangle.Height)
            {
                return true;
            }
            
            return cornerDistance <= Mathf.Pow(Radius, 2);
        }

        public override void FindCollisions(QuadTree quadTree)
        {
            List<Entity> nearbyEntities = quadTree.FindNearbyEntities(this);
            
            foreach (var nearbyEntity in nearbyEntities)
            {
                Circle circle = (Circle) nearbyEntity;
                
                if(circle == this){continue;}
                
                if (Vector2.Distance(new Vector2(PosX, PosY), new Vector2(circle.PosX, circle.PosY)) <= Radius + circle.Radius)
                {
                    Collide();
                }
            }
        }
    }
}

