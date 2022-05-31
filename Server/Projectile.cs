using System;
using System.Numerics;

namespace GameServer
{
    class Projectile
    {
        public float radius = 0.3f;
        private float speed = 4f / Constants.TICKS_PER_SEC;

        private float height = 4f;

        private float frequency = 4f;

        private Vector2 initialPos;
        public int projectileId;
        public Vector2 position;
        public Vector2 direction;

        public Constants.Trajectories type;

        public CircleCollider collider;

        public Player owner;

        public void Spawn(int _projectileId, Vector2 _position, Vector2 _direction, Constants.Trajectories _type, Player _owner)
        {
            projectileId = _projectileId;
            position = _position;
            initialPos = _position;
            direction = _direction;
            type = _type;
            owner = _owner;

            collider = new CircleCollider(position, radius);
        }

        public void Update()
        {
            if (type == Constants.Trajectories.Straight)
            {
                position += direction * speed * 2;
            }
            else
            if (type == Constants.Trajectories.Wavy)
            {
                Vector2 dist = position - initialPos;
                float totaldist = MathF.Abs(MathF.Sqrt(dist.X * dist.X + dist.Y * dist.Y));

                Vector2 perp;
                if (direction.Y == 0)
                {
                    perp = new Vector2(0, 1);
                }
                else
                {
                    perp = new Vector2(1, -direction.X / direction.Y);
                }

                float lenght = MathF.Sqrt(perp.X * perp.X + perp.Y * perp.Y);
                perp = new Vector2(perp.X / lenght, perp.Y / lenght);

                float crossZ = direction.X * perp.Y - perp.X * direction.Y;

                if (crossZ < 0)
                {
                    //HOLY JESUS CHRIST!!! SHIT GOING TO HAPPEN!!!
                    perp = new Vector2(-perp.X, -perp.Y);
                }

                float mod = position.X / direction.X;

                position += direction * speed * 2;
                position += perp * speed * MathF.Cos(totaldist * frequency) * height;
            }
            else
            if (type == Constants.Trajectories.Random)
            {
                Vector2 dist = position - initialPos;
                float totaldist = MathF.Abs(MathF.Sqrt(dist.X * dist.X + dist.Y * dist.Y));

                Vector2 perp;
                if (direction.Y == 0)
                {
                    perp = new Vector2(0, 1);
                }
                else
                {
                    perp = new Vector2(1, -direction.X / direction.Y);
                }

                float lenght = MathF.Sqrt(perp.X * perp.X + perp.Y * perp.Y);
                perp = new Vector2(perp.X / lenght, perp.Y / lenght);

                float crossZ = direction.X * perp.Y - perp.X * direction.Y;

                if (crossZ < 0)
                {
                    //HOLY JESUS CHRIST!!! SHIT GOING TO HAPPEN!!!
                    perp = new Vector2(-perp.X, -perp.Y);
                }

                float mod = position.X / direction.X;

                position += direction * speed * MathF.Sin(totaldist * frequency) * height;
                position += perp * speed * MathF.Cos(totaldist * frequency) * height;

                /*
                float angle = Utilities.RandomFloat(0, MathF.PI * 2);

                float x_ = MathF.Cos(angle);
                float y_ = MathF.Sin(angle);

                Vector2 newpos = position + new Vector2(x_,y_);

                position += newpos * speed / 2;    */            
            }else
            if (type == Constants.Trajectories.Spyral)
            {
                Vector2 dist = position - initialPos;
                float totaldist = MathF.Abs(MathF.Sqrt(dist.X * dist.X + dist.Y * dist.Y));

                Vector2 perp;
                if (direction.Y == 0)
                {
                    perp = new Vector2(0, 1);
                }
                else
                {
                    perp = new Vector2(1, -direction.X / direction.Y);
                }

                float lenght = MathF.Sqrt(perp.X * perp.X + perp.Y * perp.Y);
                perp = new Vector2(perp.X / lenght, perp.Y / lenght);

                float crossZ = direction.X * perp.Y - perp.X * direction.Y;

                if (crossZ < 0)
                {
                    //HOLY JESUS CHRIST!!! SHIT GOING TO HAPPEN!!!
                    perp = new Vector2(-perp.X, -perp.Y);
                }

                float mod = position.X / direction.X;

                position += direction * speed * MathF.Sin(totaldist * frequency) * height;
                position += perp * speed * MathF.Cos(totaldist * frequency) * height;
            }


            // TODO: Type-specific behavior.
            collider.Move(position);

            foreach (RectCollider obstacle in Server.scene.obstacles)
            {
                if (collider.CheckCollision(obstacle))
                {
                    Destroy();
                    return;
                }
            }

            foreach (Client client in Server.clients.Values)
            {
                if (client.player != null)
                {
                    if (collider.CheckCollision(client.player.collider))
                    {
                        if (client.player.Hit(owner))
                        {
                            Destroy();
                            return;
                        }
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