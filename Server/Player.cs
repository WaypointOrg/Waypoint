using System;
using System.Numerics; 

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

        public Player(int _id, string _username, Vector2 _spawnPosition)
        {
            id = _id;
            username = _username;
            position = _spawnPosition;
            rotation = 0f;

            inputs = new bool[4];

            collider = new CircleCollider(position, 0.5f);
            // collider = new RectCollider(position, new Vector2(1, 1));
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

        public void SetInput(bool[] _inputs, float _rotation)
        {
            inputs = _inputs;
            rotation = _rotation;
        }
    }
}