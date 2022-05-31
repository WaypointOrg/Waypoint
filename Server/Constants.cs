using System.Numerics;

namespace GameServer
{
    class Constants
    {
        public const int TICKS_PER_SEC = 30;
        public const float MS_PER_TICK = 1000f / TICKS_PER_SEC;

        public const float WIDTH = 16.0F;   
        public const float HEIGHT = 9.0F;

        public static Vector2 WAITING_ROOM_SPAWN = new Vector2(-34.5f, 0);

        public enum Trajectories
        {
           Straight,
           Wavy,
           Random
        }
    }
}
