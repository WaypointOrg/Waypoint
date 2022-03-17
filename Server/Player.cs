using System.Numerics; 

namespace GameServer
{
    class Player
    {
        public int id;
        public string username;

        public Vector2 position;
        public float rotation;

        public Player(int _id, string _username, Vector2 _spawnPosition)
        {
            id = _id;
            username = _username;
            position = _spawnPosition;
            rotation = 0f;
        }
    }
}