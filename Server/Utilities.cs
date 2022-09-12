using System;
using System.Numerics;
using System.Collections.Generic;

namespace GameServer
{
    class Utilities
    {
        #region ID
        public static int currentID = 0;

        public static int GetID()
        {
            currentID += 1;
            return currentID;
        }
        #endregion

        #region Random
        public static Vector2 RandomVector2(float width, float height)
        {
            Random _random = new Random();

            float _x = (float) _random.NextDouble() * width - width/2;
            float _y = (float) _random.NextDouble() * height - height/2;

            return new Vector2(_x, _y);
        }

        public static float RandomFloat(float min, float max)
        {
            Random _random = new Random();
            return min + (float) _random.NextDouble() * (max - min);
        }

        public static int RandomInt(int _max)
        {
            Random _random = new Random();
            return _random.Next(_max);
        }

        public static int RandomSign()
        {
            int rndm = RandomInt(1);

            if(rndm == 0)
            {
                return 1;
            }else
            {
                return -1;
            }
        }

        // Returns position the furthest away from all items and players among 20 random free positions
        public static Vector2 RandomFreeGoodPosition(float _radius)
        {
            Vector2 bestPosition = Vector2.Zero;
            float bestDistance = 0f;
            for (int i = 0; i < 20; i++)
            {
                Vector2 _position = RandomFreeCirclePositionInMap(_radius);
                float smallestDistance = 10000f;
                foreach (Item item in Server.items.Values)
                {
                    float distance = Vector2.Distance(item.position, _position);
                    smallestDistance = MathF.Min(smallestDistance, distance);
                }
                foreach (Client client in Server.clients.Values)
                {
                    if (client.player == null) continue;
                    float distance = Vector2.Distance(client.player.position, _position);
                    smallestDistance = MathF.Min(smallestDistance, distance);
                }
                if (bestDistance < smallestDistance)
                {
                    bestDistance = smallestDistance;
                    bestPosition = _position;
                }
            }
            return bestPosition;
        }

        public static Vector2 RandomFreeCirclePositionInMap(float _radius)
        {
            Vector2 position = new Vector2();
            bool _placeable = false;
            while (!_placeable)
            {
                position = Utilities.RandomVector2(Constants.WIDTH, Constants.HEIGHT);
                CircleCollider _collider = new CircleCollider(position, _radius);

                _placeable = !IsCollidingWithObstacles(_collider);
            }
            return position;
        }
        #endregion

        public static bool IsCollidingWithObstacles(RectCollider collider)
        {
            foreach (RectCollider obstacle in Server.maps[Server.currentMapId].obstacles)
            {
                if (collider.CheckCollision(obstacle))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsCollidingWithObstacles(CircleCollider collider)
        {
            foreach (RectCollider obstacle in Server.maps[Server.currentMapId].obstacles)
            {
                if (collider.CheckCollision(obstacle))
                {
                    return true;
                }
            }
            return false;
        }
        // public static dynamic Insert(this Dictionary<int, dynamic> dict, Type element)
        // {
        //     // Dictionary<int, string> d = new Dictionary<int, string>();
        //     int _index = 0;
        //     while (true)
        //     {
        //         if (dict.TryAdd(_index, new element()))
        //         {
        //             // TODO: shoot correct projectile type
        //             Projectile _projectile = Server.projectiles[_index];
        //             // TODO: Reference player in projectile for counting kills
        //             _projectile.Spawn(_index, position + direction * (radius + _projectile.radius), direction, Projectile.ProjectileType.normal);
        //             ServerSend.ProjectileSpawned(_projectile);
        //             break;
        //         }
        //         _index += 1;
        // }
    }
}