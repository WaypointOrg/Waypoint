using System;
using System.Numerics; 

namespace GameServer
{
    class Item
    {
        private float radius = 0.5f;

        public int itemId;
        public Vector2 position;

        public int type;

        public CircleCollider collider;

        public void Spawn(int _itemId)
        {
            itemId = _itemId;
            type = (int)Utilities.RandomInt(Constants.guns.Length);          
            position = Utilities.RandomFreeGoodPosition(radius);
            collider = new CircleCollider(position, radius);
            ServerSend.ItemSpawned(this);
        }
    }
}