using System;
using System.Numerics; 
using System.Collections.Generic; 

namespace GameServer
{
    class Player
    {
        public float radius = 0.5f;
        public int id;
        public string username;

        public Vector2 position;
        public float rotation;

        private float moveSpeed = 4f / Constants.TICKS_PER_SEC;
        private bool[] inputs;

        public CircleCollider collider;
        // public RectCollider collider;

        public List<Item> items;

        public Player(int _id, string _username, Vector2 _spawnPosition)
        {
            id = _id;
            username = _username;
            position = _spawnPosition;
            rotation = 0f;

            inputs = new bool[4];

            collider = new CircleCollider(position, radius);
            // collider = new RectCollider(position, new Vector2(1, 1));
            items = new List<Item>();
        }

        public void Update()
        {
            Vector2 _inputDirection = Vector2.Zero;
            if (inputs[0])
            {
                _inputDirection.Y += 1;
            }
            if (inputs[1])
            {
                _inputDirection.Y -= 1;
            }
            if (inputs[2])
            {
                _inputDirection.X -= 1;
            }
            if (inputs[3])
            {
                _inputDirection.X += 1;
            }
            
            if (_inputDirection != Vector2.Zero)
            {
                Move(Vector2.Normalize(_inputDirection));
            }

            ServerSend.PlayerRotation(this);
            
            AttemptPickUp();
        }

        private void Move(Vector2 _inputDirection)
        {
            Vector2 new_position = position + _inputDirection * moveSpeed;
            collider.Move(new_position);
            foreach (RectCollider obstacle in Server.scene.obstacles)
            {
                if (collider.CheckCollision(obstacle)){
                    return;
                }
            }

            position = new_position;
            ServerSend.PlayerPosition(this);
        }

        private void AttemptPickUp()
        {
            foreach (Item item in Server.items.Values)
            {
                if (collider.CheckCollision(item.collider)){
                    Server.items.Remove(item.itemId);
                    items.Add(item);

                    // TODO: Check if player already has item.
                    Console.WriteLine($"Player {username} has {items.Count} items.");

                    ServerSend.ItemPickedUp(item, this);
                }
            } 
        }

        public void SetInput(bool[] _inputs, float _rotation)
        {
            inputs = _inputs;
            rotation = _rotation;
        }

        public void Shoot()
        {
            int _index = 0;
            while (true)
            {
                if (Server.projectiles.TryAdd(_index, new Projectile()))
                {
                    Vector2 _direction = new Vector2(
                        (float) Math.Cos(rotation * (Math.PI / 180)),
                        (float) Math.Sin(rotation * (Math.PI / 180)));

                    // TODO: shoot correct projectile type
                    Projectile _projectile = Server.projectiles[_index];
                    _projectile.Spawn(_index, position + _direction * (radius + _projectile.radius), _direction, Projectile.ProjectileType.normal);
                    ServerSend.ProjectileSpawned(_projectile);
                    break;
                }
                _index += 1;
            }
        }
    }
}