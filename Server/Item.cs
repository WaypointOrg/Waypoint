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
            Orange = 0,
            Green  = 1,
            Pink   = 2,
        }
        public ItemType type;

        public CircleCollider collider;

        public void Spawn(int _itemId)
        {
            itemId = _itemId;
            type = (Item.ItemType) Utilities.RandomInt(Enum.GetNames(typeof(Item.ItemType)).Length);            
            position = Utilities.RandomFreeCirclePosition(radius);
            collider = new CircleCollider(position, radius);
            ServerSend.ItemSpawned(this);

            // Console.WriteLine($"Spawned item {type} at ({position.X}, {position.Y})");
        }
    }
}