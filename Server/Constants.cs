using System.Numerics;
using System.Collections.Generic;

namespace GameServer
{
    class Constants
    {

        public static List<string> defaultNames = new List<string>() {
            "Alice",
            "Bob",
            "Vroumm",
            "Theel",
            "Rothkir",
            "Daru",
            "Tractor",
            "YOURSELF",
            "DeltaX",
            "TMS"
        };

        public const int TICKS_PER_SEC = 30;
        public const float MS_PER_TICK = 1000f / TICKS_PER_SEC;

        public const float WIDTH = 25f;
        public const float HEIGHT = 18f;

        public static Vector2 WAITING_ROOM_SPAWN = new Vector2(5, 3);
        // TODO: Use waiting room spawn points
        // void SendToWaitingRoom() ?

        // TODO: Clean up this 
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
        new Gun("minigun", 1, 1, 1, 0.5f, 10),
        new Gun("carabine", 1, 2, 1.5f, 1f, 7),
        new Gun("rifle", 0, 1, 1, 0.1f, 15),
        new Gun("flame thrower", 2, 5, 0.5f, 1.5f, 10),
        new Gun("blowgun", 0, 21, 0.5f, 3f, 3),
        new Gun("randomer", 3, 2, 1, 1f, 10),
        new Gun("cahos", 3, 36, 1, 1f, 1),
        new Gun("diagunal", 4, 1, 2, 0.5f, 20),
        new Gun("miz", 0, 2, 3, 0.5f, 10)};
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
