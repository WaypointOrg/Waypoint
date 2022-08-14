using System;
using System.Numerics;
using System.Collections.Generic;


namespace GameServer
{
    class Turret
    {
        public int id;

        public float radius = 0.3f;
        public CircleCollider collider;
        public Vector3 position;
        public float rotation;

        public Player owner;

        public void Initialize(int _id, Vector3 _position, float _rotation, Player _owner)
        {
            id = _id;
            position = _position;
            rotation = _rotation;
            owner = _owner;
        }

        /*void Update()
        {
            Dictionary<float, Vector3> dists = new Dictionary<float, Vector3>();
            Vector3 target;

            foreach(KeyValuePair<int, Client> clientPair in Server.clients)
            {
                if(clientPair.value != owner)
                {
                    Vector3 dist = clientPair.value.player.position - position;
                    dists.Add(dist, clientPair.value.player.position);
                }
            }
            
            if(dists.Count != 0)
            {
                target = dists[dists.Keys.Min()];
                //todo shoot
            }
        }*/
    }
}