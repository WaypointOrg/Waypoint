using System;
using System.Numerics;
using System.Collections.Generic;

namespace GameServer
{
    public class Player
    {
        public float radius = 0.5f;
        public int id;
        public string username;

        public Vector2 position;
        public float rotation;

        public Gun currentGun = Constants.guns[0];
        public int myAmmo = Constants.guns[0].ammo;
        bool canShoot = true;
        float shootDelay = Constants.guns[0].cooldown * Constants.TICKS_PER_SEC;
        //public float dmg;

        private float moveSpeed = 4f / Constants.TICKS_PER_SEC;
        private bool[] inputs;

        public CircleCollider collider;

        // Time before respawn after a hit
        public int respawnTime = (int)(0.5 * Constants.TICKS_PER_SEC);
        public int respawnTimer;
        public bool isRespawning;

        // Duration of invicibility after respawn
        public int invicibilityTime = (int)(1.5 * Constants.TICKS_PER_SEC);
        public int invicibilityTimer;

        public Player(int _id, string _username, Vector2 _spawnPosition)
        {
            id = _id;
            username = _username;
            position = _spawnPosition;
            rotation = 0f;

            inputs = new bool[4];

            collider = new CircleCollider(position, radius);

            invicibilityTimer = 0;
            respawnTimer = 0;
            isRespawning = false;
            ServerSend.PlayerAmmo(this);
        }

        public void Update()
        {
            Vector2 _inputDirection = Vector2.Zero;
            if (inputs[0])
            {
                _inputDirection.Y += 1;
            }
            if (inputs[1])
            {
                _inputDirection.Y -= 1;
            }
            if (inputs[2])
            {
                _inputDirection.X -= 1;
            }
            if (inputs[3])
            {
                _inputDirection.X += 1;
            }

            if (_inputDirection != Vector2.Zero)
            {
                _inputDirection = Vector2.Normalize(_inputDirection);
            }

            Move(_inputDirection);

            ServerSend.PlayerRotation(this);

            AttemptPickUp();

            if (invicibilityTimer > 0) invicibilityTimer -= 1;
            if (respawnTimer > 0) respawnTimer -= 1;

            if (isRespawning && respawnTimer == 0)
            {
                ServerSend.PlayerRespawned(this);

                // TODO: Respawn on respawn point ?
                Vector2 _position = Utilities.RandomFreeCirclePositionInMap(radius);
                Teleport(_position);

                invicibilityTimer = invicibilityTime;
                isRespawning = false;
            }

            if(!canShoot)
            {
                shootDelay -= 1;
            }

            if(shootDelay <= 0)
            {
                shootDelay = currentGun.cooldown * Constants.TICKS_PER_SEC;
                canShoot = true;
            }
        }

        public void Teleport(Vector2 _position)
        {
            position = _position;
            collider.position = position;

            ServerSend.PlayerPosition(this);
        }

        private void Move(Vector2 _direction)
        {
            Vector2 movement = Vector2.Zero;
            Vector2 movement_x = new Vector2(_direction.X * moveSpeed, 0);
            Vector2 movement_y = new Vector2(0, _direction.Y * moveSpeed);

            collider.Move(position + movement_x);
            if (!Utilities.IsCollidingWithObstacles(collider))
            {
                movement += movement_x;
            }
            
            // Console.WriteLine(movement_y);
            collider.Move(position + movement_y);
            if (!Utilities.IsCollidingWithObstacles(collider))
            {
                movement += movement_y;
            }

            position = position + movement;
            ServerSend.PlayerPosition(this);
        }

        public bool Hit(Player _by)
        {
            if (invicibilityTimer > 0 || isRespawning) return false;
            if (!Server.gameStarted) return false;

            // Friendly Fire
            if (_by.id == id) return false;

            ServerSend.PlayerHit(this, _by);
            respawnTimer = respawnTime;
            isRespawning = true;

            return true;
        }

        private void AttemptPickUp()
        {
            foreach (Item item in Server.items.Values)
            {
                if (collider.CheckCollision(item.collider))
                {
                    Server.items.Remove(item.itemId);

                    //BIG TODO
                    if(Constants.guns[item.type]._name == currentGun._name)
                    {
                        myAmmo += Constants.guns[item.type].ammo;
                    }else
                    {
                        currentGun = Constants.guns[item.type];
                        myAmmo = currentGun.ammo;
                        shootDelay = currentGun.cooldown * Constants.TICKS_PER_SEC;
                        canShoot = true;
                    }
                    
                    ServerSend.PlayerAmmo(this);
            
                    Console.WriteLine(username + " now has a " + currentGun._name);

                    ServerSend.ItemPickedUp(item, this);
                }
            }
        }

        public void SetInput(bool[] _inputs, float _rotation)
        {
            inputs = _inputs;
            rotation = _rotation;
        }

        public void Shoot()
        { 
            if(!canShoot)
            {
                return;
            }else
            {
                canShoot = false;
            }

            if(myAmmo <= 0)
            {
                return;
            }else
            {
                myAmmo -= 1;
                ServerSend.PlayerAmmo(this);
            }
            //todo: get shotgun & spawn projectile + ask for trajectory

            Vector2 _direction = new Vector2(
            (float)Math.Cos(rotation * (Math.PI / 180)),
            (float)Math.Sin(rotation * (Math.PI / 180)));

            float number = currentGun.bulletNumber;
            float angle = 10;

            if (number % 2 == 1)
            {
                //the number is odd
                float side = MathF.Floor(number / 2);

                SummonProjectile(_direction);

                for (int i = 0; i < side; i++)
                {
                    Vector2 deltaDir1 = new Vector2((float)Math.Cos((rotation + angle * (i + 1)) * (Math.PI / 180)), (float)Math.Sin((rotation + angle * (i + 1)) * (Math.PI / 180)));
                    Vector2 deltaDir2 = new Vector2((float)Math.Cos((rotation - angle * (i + 1)) * (Math.PI / 180)), (float)Math.Sin((rotation - angle * (i + 1)) * (Math.PI / 180)));
                    SummonProjectile(deltaDir1);
                    SummonProjectile(deltaDir2);
                }
            }
            else
            {
                //the number is even
                float side = number / 2;

                for (int i = 0; i < side; i++)
                {
                    Vector2 deltaDir1 = new Vector2((float)Math.Cos((rotation + angle * (i + 1)) * (Math.PI / 180)), (float)Math.Sin((rotation + angle * (i + 1)) * (Math.PI / 180)));
                    Vector2 deltaDir2 = new Vector2((float)Math.Cos((rotation - angle * (i + 1)) * (Math.PI / 180)), (float)Math.Sin((rotation - angle * (i + 1)) * (Math.PI / 180)));
                    SummonProjectile(deltaDir1);
                    SummonProjectile(deltaDir2);
                }
            }
        }

        void SummonProjectile(Vector2 direction)
        {
            int id_ = Utilities.GetID();

            if (Server.projectiles.TryAdd(id_, new Projectile()))
            {
                // TODO: shoot correct projectile type
                Projectile _projectile = Server.projectiles[id_];
                _projectile.Spawn(
                    id_,
                    position + direction * (radius + _projectile.radius),
                    direction,
                    currentGun.trajectory,
                    currentGun.bulletSpeed,
                    this);
                ServerSend.ProjectileSpawned(_projectile);
            }
        }



        /*
        void Shoot()
        {
            number = proj.number;
            angle = proj.angle;

            if (number % 2 == 1)
            {
                //the number is odd
                float side = Mathf.Floor(number / 2);

                GameObject pr1 = Instantiate(projectile, shooPos.position, transform.rotation);
                projs.Add(pr1);

                for (int i = 0; i < side; i++)
                {
                    GameObject pr2 = Instantiate(projectile, shooPos.position, Quaternion.Euler(new Vector3(0, 0, transform.eulerAngles.z + angle * (i + 1))));
                    GameObject pr3 = Instantiate(projectile, shooPos.position, Quaternion.Euler(new Vector3(0, 0, transform.eulerAngles.z - angle * (i + 1))));

                    projs.Add(pr2);
                    projs.Add(pr3);
                }
            }
            else
            {
                //the number is even
                float side = number / 2;

                for (int i = 0; i < side; i++)
                {
                    GameObject pr4 = Instantiate(projectile, shooPos.position, Quaternion.Euler(new Vector3(0, 0, transform.eulerAngles.z + angle * i + angle / 2)));
                    GameObject pr5 = Instantiate(projectile, shooPos.position, Quaternion.Euler(new Vector3(0, 0, transform.eulerAngles.z - angle * i - angle / 2)));

                    projs.Add(pr4);
                    projs.Add(pr5);
                }
            }

            foreach (GameObject p in projs)
            {
                Projectile projScript = p.GetComponent<Projectile>();
                projScript.updateValues(proj.speed, proj.size, (int)proj.number, proj.angle, proj.frequency, proj.height, proj.seek);
            }

            projs.Clear();
            spellManager.ResetProj();
        }
        */
    }
}