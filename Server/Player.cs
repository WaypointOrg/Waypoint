using System;
using System.Numerics; 
using System.Collections.Generic; 

namespace GameServer
{
    class Player
    {
        public float radius = 0.5f;
        public int id;
        public string username;

        public Vector2 position;
        public float rotation;

        private float moveSpeed = 4f / Constants.TICKS_PER_SEC;
        private bool[] inputs;

        public CircleCollider collider;

        public List<Item> items;
        
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
            items = new List<Item>();

            invicibilityTimer = 0;
            respawnTimer = 0;
            isRespawning = false;
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

                Vector2 _position = Utilities.RandomFreeCirclePosition(radius);
                Teleport(_position);

                invicibilityTimer = invicibilityTime;
                isRespawning = false;
            }
        }

        public void Teleport(Vector2 _position)
        {
            position = _position;
            collider.position = position;

            ServerSend.PlayerPosition(this);
        }

        private void Move(Vector2 _inputDirection)
        {
            Vector2 new_position = position + _inputDirection * moveSpeed;
            collider.Move(new_position);
            foreach (RectCollider obstacle in Server.scene.obstacles)
            {
                if (collider.CheckCollision(obstacle)){
                    return;
                }
            }

            position = new_position;
            ServerSend.PlayerPosition(this);
        }

        public bool Hit(Player _by)
        {
            if (invicibilityTimer > 0 || isRespawning) return false;

            // Friendly Fire
            // if (_by.id == id) return false;

            ServerSend.PlayerHit(this, _by);
            respawnTimer = respawnTime;
            isRespawning = true;

            return true;
        }

        private void AttemptPickUp()
        {
            foreach (Item item in Server.items.Values)
            {
                if (collider.CheckCollision(item.collider)){
                    Server.items.Remove(item.itemId);
                    items.Add(item);

                    // TODO: Check if player already has item.
                    Console.WriteLine($"Player {username} has {items.Count} items.");

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
            Vector2 _direction = new Vector2(
                        (float) Math.Cos(rotation * (Math.PI / 180)),
                        (float) Math.Sin(rotation * (Math.PI / 180)));

            float number = 5;
            float angle = 10;

            if (number % 2 == 1)
            {
                //the number is odd
                float side = MathF.Floor(number / 2);
                
                SummonProjectile(_direction);

                for (int i = 0; i < side; i++)
                {
                    Vector2 deltaDir1 = new Vector2((float) Math.Cos((rotation + angle * (i + 1)) * (Math.PI / 180)), (float) Math.Sin((rotation + angle * (i + 1)) * (Math.PI / 180)));
                    Vector2 deltaDir2 = new Vector2((float) Math.Cos((rotation - angle * (i + 1)) * (Math.PI / 180)), (float) Math.Sin((rotation - angle * (i + 1)) * (Math.PI / 180)));
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
                    Vector2 deltaDir1 = new Vector2((float) Math.Cos((rotation + angle * (i + 1)) * (Math.PI / 180)), (float) Math.Sin((rotation + angle * (i + 1)) * (Math.PI / 180)));
                    Vector2 deltaDir2 = new Vector2((float) Math.Cos((rotation - angle * (i + 1)) * (Math.PI / 180)), (float) Math.Sin((rotation - angle * (i + 1)) * (Math.PI / 180)));
                    SummonProjectile(deltaDir1);
                    SummonProjectile(deltaDir2);
                }
            }
        }

        void SummonProjectile(Vector2 direction)
        {
            int _index = 0;
            while (true)
            {
                if (Server.projectiles.TryAdd(_index, new Projectile()))
                {
                    // TODO: shoot correct projectile type
                    Projectile _projectile = Server.projectiles[_index];
                    _projectile.Spawn(
                        _index,
                        position + direction * (radius + _projectile.radius),
                        direction,
                        Projectile.ProjectileType.normal,
                        this);
                    ServerSend.ProjectileSpawned(_projectile);
                    break;
                }
                _index += 1;
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