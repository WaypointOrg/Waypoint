using System;
using System.Numerics; 

namespace GameServer
{
    class Item
    {
        private float radius = 0.5f;

        public int itemId;
        public Vector2 position;

        public Constants.Trajectories type;

        public CircleCollider collider;

        public void Spawn(int _itemId)
        {
            itemId = _itemId;
            type = (Constants.Trajectories) Utilities.RandomInt(Enum.GetNames(typeof(Constants.Trajectories)).Length);            
            position = Utilities.RandomFreeCirclePosition(radius);
            collider = new CircleCollider(position, radius);
            ServerSend.ItemSpawned(this);

            // Console.WriteLine($"Spawned item {type} at ({position.X}, {position.Y})");
        }
    }
}