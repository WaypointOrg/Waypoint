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
           Random,
           Spyral
        }

        public static Gun[] guns = new Gun[]
        {new Gun("pistol", 0, 1, 1, 1, 10),
        new Gun("shotgun", 0, 5, 1, 2, 5),
        new Gun("sniper", 0, 1, 3, 2, 3),
        new Gun("minigun", 1, 1, 1, 0.5f, 10)};
    }

    public class Gun
    {
        public string _name;
        public int trajectory;
        public int bulletNumber;
        public float bulletSpeed;
        public float cooldown;
        public int ammo;

        public Gun(string name_, int trajectory_, int bulletNumber_, float bulletSpeed_, float cooldown_, int ammo_)
        {
            _name = name_;
            trajectory = trajectory_;
            bulletNumber = bulletNumber_;
            bulletSpeed = bulletSpeed_;
            cooldown = cooldown_;
            ammo = ammo_;
        }
    }
}
