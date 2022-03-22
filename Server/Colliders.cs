using System;
using System.Numerics;

namespace GameServer
{
    public class RectCollider
    {
        public Vector2 position;
        public Vector2 scale;

        public RectCollider(Vector2 _position, Vector2 _scale)
        {
            position = _position;
            scale = _scale;
        }

        public void Move(Vector2 _position)
        {
            position = _position;
        }

        public bool CheckCollision(RectCollider collider)
        {   
            return (position.X + scale.X/2 > collider.position.X - collider.scale.X/2 && 
                    position.X - scale.X/2 < collider.position.X + collider.scale.X/2 &&
                    position.Y + scale.Y/2 > collider.position.Y - collider.scale.Y/2 && 
                    position.Y - scale.Y/2 < collider.position.Y + collider.scale.Y/2);
        }

        public bool CheckCollision(CircleCollider collider)
        {
            return collider.CheckCollision(this);
        }
    }


    public class CircleCollider
    {
        public Vector2 position;
        public float radius;

        public CircleCollider(Vector2 _position, float _radius)
        {
            position = _position;
            radius = _radius;
        }

        public void Move(Vector2 _position)
        {
            position = _position;
        }

        public bool CheckCollision(RectCollider rect)
        {
            float testX = Math.Clamp(position.X, rect.position.X - rect.scale.X/2, rect.position.X + rect.scale.X/2);
            float testY = Math.Clamp(position.Y, rect.position.Y - rect.scale.Y/2, rect.position.Y + rect.scale.Y/2);

            float distX = position.X - testX;
            float distY = position.Y - testY;
            float distance = (float) Math.Sqrt((distX*distX) + (distY*distY));

            return distance <= radius;
        }

        public bool CheckCollision(CircleCollider collider)
        {
            float distX = position.X - collider.position.X;
            float distY = position.Y - collider.position.Y;
            float distance = (float) Math.Sqrt((distX*distX) + (distY*distY));
            return distance <= radius + collider.radius;
        }
    }
}