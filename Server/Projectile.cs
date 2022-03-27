using System;
using System.Numerics; 

namespace GameServer
{
    class Projectile
    {
        public float radius = 0.3f;
        private float speed = 1f / Constants.TICKS_PER_SEC;

        private float height = 4f;

        private float frequency = 4f;

        private Vector2 initialPos;
        public int projectileId;
        public Vector2 position;
        public Vector2 direction;

        public enum ProjectileType
        {
            // TODO: Add true projectile types
            normal
        }
        public ProjectileType type;

        public CircleCollider collider;

        public void Spawn(int _projectileId, Vector2 _position, Vector2 _direction, ProjectileType _type)
        {
            projectileId = _projectileId;
            position = _position;
            initialPos = _position;
            direction = _direction;
            type = _type;

            collider = new CircleCollider(position, radius);
        }

        public void Update()
        {
            // TODO: Type-specific behavior.
            Vector2 dist = position - initialPos;
            float totaldist = MathF.Sqrt(dist.X * dist.X + dist.Y * dist.Y);

            Vector2 perp;
            if(direction.Y == 0)
            {
                perp = new Vector2(0,1);
            }else
            {
                perp = new Vector2(1, -direction.X/direction.Y);
            }

            float lenght = MathF.Sqrt(perp.X * perp.X + perp.Y * perp.Y);
            perp = new Vector2(perp.X/lenght, perp.Y/lenght);

            float mod = position.X/direction.X;

            position += direction * speed * 2;
            position += perp * speed * MathF.Cos(totaldist * frequency) * height;
            collider.Move(position);

            foreach (RectCollider obstacle in Server.scene.obstacles)
            {
                if (collider.CheckCollision(obstacle)){
                        Destroy();
                    return;
                }
            }

            foreach (Client client in Server.clients.Values)
            {
                if (client.player != null)
                {
                    if (collider.CheckCollision(client.player.collider)){
                        
                        // TODO: Player Damage

                        Destroy();
                        return;
                    }
                }
            }
            ServerSend.ProjectilePosition(this);
        }

        public void Destroy()
        {
            ServerSend.ProjectileDestroy(this);
            Server.projectiles.Remove(projectileId);
        }
    }
}