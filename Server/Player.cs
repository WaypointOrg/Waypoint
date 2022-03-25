using System;
using System.Numerics; 
using System.Collections.Generic; 

namespace GameServer
{
    class Player
    {
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

            collider = new CircleCollider(position, 0.5f);
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

            Move(_inputDirection);
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
            ServerSend.PlayerRotation(this);
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
    }
}