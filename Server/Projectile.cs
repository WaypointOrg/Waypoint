using System;
using System.Numerics; 

namespace GameServer
{
    class Projectile
    {
        public float radius = 0.3f;
        private float speed = 10f / Constants.TICKS_PER_SEC;

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
            direction = _direction;
            type = _type;

            collider = new CircleCollider(position, radius);
        }

        public void Update()
        {
            // TODO: Type-specific behavior.
            position += direction * speed;
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