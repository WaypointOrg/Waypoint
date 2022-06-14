using System;
using System.Numerics;
using System.Collections.Generic;

namespace GameServer
{
    class Utilities
    {
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

        public static Vector2 RandomFreeCirclePositionInMap(float _radius)
        {
            Vector2 position = new Vector2();
            bool _placeable = false;
            while (!_placeable)
            {
                position = Utilities.RandomVector2(Constants.WIDTH, Constants.HEIGHT) + Server.scene.mapCenters[Server.currentMapId];
                CircleCollider _collider = new CircleCollider(position, _radius);

                _placeable = !IsCollidingWithObstacles(_collider);
                // TODO: Not spawn close to players
            }
            return position;
        }
        #endregion

        public static bool IsCollidingWithObstacles(RectCollider collider)
        {
            foreach (RectCollider obstacle in Server.scene.obstacles)
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
            foreach (RectCollider obstacle in Server.scene.obstacles)
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