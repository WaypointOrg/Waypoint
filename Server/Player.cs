using System.Numerics; 

namespace GameServer
{
    class Player
    {
        public int id;
        public string username;

        public Vector2 position;
        public float rotation;

        private float moveSpeed = 5f / Constants.TICKS_PER_SEC;
        private bool[] inputs;

        public Player(int _id, string _username, Vector2 _spawnPosition)
        {
            id = _id;
            username = _username;
            position = _spawnPosition;
            rotation = 0f;

            inputs = new bool[4];
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
            position += _inputDirection * moveSpeed;

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