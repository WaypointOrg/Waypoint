using System;
using System.Numerics; 

namespace GameServer
{
    class Item
    {
        private float radius = 0.5f;

        public int itemId;
        public Vector2 position;

        public enum ItemType
        {
            Orange,
            Green,
            Pink
        }
        public ItemType type;

        public CircleCollider collider;

        public void Spawn(int _itemId)
        {
            itemId = _itemId;

            Random _random = new Random();
            type = (Item.ItemType) _random.Next(Enum.GetNames(typeof(Item.ItemType)).Length);

            bool _placeable = false;
            while (!_placeable)
            {
                float _x = (float) _random.NextDouble() * Constants.WIDTH - Constants.WIDTH/2;
                float _y = (float) _random.NextDouble() * Constants.HEIGHT - Constants.HEIGHT/2;

                position = new Vector2(_x, _y);
                collider = new CircleCollider(position, radius);

                _placeable = true;
                foreach (RectCollider obstacle in Server.scene.obstacles)
                {
                    if (collider.CheckCollision(obstacle)){
                        _placeable = false;
                        break;
                    }
                }

                // TODO: Not spawn close to player or spawner. Create isPlaceable function.
            }

            ServerSend.ItemSpawned(this);

            // Console.WriteLine($"Spawned item {type} at ({position.X}, {position.Y})");
        }
    }
}