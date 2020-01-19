using System.Collections.Generic;
using Manager;
using UnityEngine;

namespace Tree
{
    public class QuadTree
    {
        private QuadTree _topLeft;
        private QuadTree _topRight;
        private QuadTree _bottomLeft;
        private QuadTree _bottomRight;
        private bool divided;

        public Rectangle Rectangle { get; set; }
        public int Capacity { get; set; }

        public List<Entity.Entity> Entities { get; set; }

        public QuadTree(Rectangle rectangle, int capacity)
        {
            Rectangle = rectangle;
            Capacity = capacity;
            Entities = new List<Entity.Entity>();
            divided = false;
        }

        private void Subdivide()
        {
            Rectangle topLeftRect = new Rectangle(Rectangle.PosX + Rectangle.Width/2, Rectangle.PosY + Rectangle.Height/2, Rectangle.Width/2, Rectangle.Height/2);
            _topLeft = new QuadTree(topLeftRect, Capacity);
            Rectangle topRightRect = new Rectangle(Rectangle.PosX - Rectangle.Width/2, Rectangle.PosY + Rectangle.Height/2, Rectangle.Width/2, Rectangle.Height/2);
            _topRight = new QuadTree(topRightRect, Capacity);
            Rectangle bottomLeftRect = new Rectangle(Rectangle.PosX + Rectangle.Width/2, Rectangle.PosY - Rectangle.Height/2, Rectangle.Width/2, Rectangle.Height/2);
            _bottomLeft = new QuadTree(bottomLeftRect, Capacity);
            Rectangle bottomRightRect = new Rectangle(Rectangle.PosX - Rectangle.Width/2, Rectangle.PosY - Rectangle.Height/2, Rectangle.Width/2, Rectangle.Height/2);
            _bottomRight = new QuadTree(bottomRightRect, Capacity);
            
            LineDrawer.Instance.DrawLine(new Vector3(Rectangle.PosX, Rectangle.PosY), new Vector3(Rectangle.PosX + Rectangle.Width, Rectangle.PosY), new Vector3(Rectangle.PosX - Rectangle.Width, Rectangle.PosY));
            LineDrawer.Instance.DrawLine(new Vector3(Rectangle.PosX, Rectangle.PosY), new Vector3(Rectangle.PosX, Rectangle.PosY + Rectangle.Height), new Vector3(Rectangle.PosX, Rectangle.PosY - Rectangle.Height));
            
            divided = true;
        }

        public List<Entity.Entity> FindNearbyEntities(Entity.Entity entity)
        {
            List<Entity.Entity> found = new List<Entity.Entity>();
            
            if (entity.Intersects(Rectangle))
            {
                if(divided){
                    found.AddRange(_topLeft.FindNearbyEntities(entity));
                    found.AddRange(_topRight.FindNearbyEntities(entity));
                    found.AddRange(_bottomLeft.FindNearbyEntities(entity));
                    found.AddRange(_bottomRight.FindNearbyEntities(entity));
                }
                
                found.AddRange(Entities);
            }
            
            return found;
        }

        public bool Insert(Vector3 point, Entity.Entity entity)
        {
            if (Entities.Count < Capacity)
            {
                if (Rectangle.Contains(point))
                {
                    Entities.Add(entity);
                    entity.ParentQuadTree = this;
                    return true;
                } 
            }
            else
            {
                if (!divided)
                {
                    Subdivide();
                }
                if (_topLeft.Insert(point, entity) || _topRight.Insert(point, entity) || 
                    _bottomLeft.Insert(point, entity) || _bottomRight.Insert(point, entity))
                {
                    return true;
                }
            }
            
            return false;
        }

        public void FindCollisions(QuadTree quadTree)
        {
            foreach (var entity in Entities)
            {
                entity.FindCollisions(quadTree);
            }

            if (divided)
            {
                _topLeft.FindCollisions(quadTree);
                _topRight.FindCollisions(quadTree);
                _bottomLeft.FindCollisions(quadTree);
                _bottomRight.FindCollisions(quadTree);
            }
        }

        public void Clear()
        {
            if (divided)
            {
                _topLeft.Clear();
                _topLeft = null;
                
                _topRight.Clear();
                _topRight = null;
                
                _bottomLeft.Clear();
                _bottomLeft = null;
                
                _bottomRight.Clear();
                _bottomRight = null;
                
                divided = false;
            }
            
            foreach (var entity in Entities)
            {
                GameObject.Destroy(entity.gameObject);
            }
            Entities.Clear();
        }
    }

    public class Rectangle
    {
        public float PosX { get; set; }
        public float PosY { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }

        public Rectangle(float x, float y, float width, float height)
        {
            PosX = x;
            PosY = y;
            Width = width;
            Height = height;
        }

        public bool Contains(Vector3 position)
        {
            return position.x <= PosX + Width && position.x >= PosX - Width && position.y <= PosY + Height &&
                   position.y >= PosY - Height;
        }
    }
}

