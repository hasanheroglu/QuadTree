using System.Collections.Generic;
using Manager;
using Tree;
using UnityEngine;

namespace Entity
{ 
    public class Rectangle : Entity
    {
        public float Width { get; set; }
        public float Height { get; set; }

        public override void SetPosition(Vector3 position)
        {
            gameObject.transform.SetPositionAndRotation(position, Quaternion.identity);
            PosX = position.x;
            PosY = position.y;
        }

        public override void SetSize(float width, float height)
        {
            Width = width/2;
            Height = height/2;
            StartCoroutine(Tween.Instance.Grow(gameObject, Vector3.zero, new Vector3(width, height, 1), 0.1f));
        }

        public override bool Intersects(Tree.Rectangle rectangle)
        {
            return !(
                    PosX + Width < rectangle.PosX - rectangle.Width ||
                    PosX - Width > rectangle.PosX + rectangle.Width ||
                    PosY - Height > rectangle.PosY + rectangle.Height ||
                    PosY + Height < rectangle.PosY - rectangle.Height
                );
        }

        public override void FindCollisions(QuadTree quadTree)
        {
            List<Entity> nearbyEntities = quadTree.FindNearbyEntities(this);
            
            if (nearbyEntities.Count == 0)
            {
                return;
            }
            
            foreach (var nearbyEntity in nearbyEntities)
            {
                Rectangle rectangle = (Rectangle) nearbyEntity;
                
                if(rectangle == this){continue;}
                
                if (!(
                    PosX + Width < rectangle.PosX - rectangle.Width ||
                    PosX - Width > rectangle.PosX + rectangle.Width ||
                    PosY - Height > rectangle.PosY + rectangle.Height ||
                    PosY + Height < rectangle.PosY - rectangle.Height
                ))
                {
                   Collide();
                }
            }
        }
    }
}

