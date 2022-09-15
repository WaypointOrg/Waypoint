using System;
using System.Numerics;
using System.Collections.Generic;

namespace GameServer
{
    class GameLogic
    {
        public static void Update()
        {
            // Starting
            if (StartingConditionsMet())
            {
                StartGame();
            }

            // Ending
            if (Server.gameStarted)
            {
                Server.gameTime -= 1;
                if (Server.gameTime == 0)
                {
                    EndGame();
                }
            }

            // Update clients
            foreach (Client _client in Server.clients.Values)
            {
                if (_client.player == null) continue;
                _client.player.Update();
            }

            // Add items
            if (Server.gameStarted)
            {
                if (Server.nextItemTime <= 0 &&
                    Server.items.Count < Server.maxItems)
                {
                    AddItem();
                }
                Server.nextItemTime -= 1;
            }

            // Update projectiles
            foreach (Projectile projectile in Server.projectiles.Values)
            {
                projectile.Update();
            }

            ThreadManager.UpdateMain();
        }

        private static void AddItem()
        {
            int _id = Utilities.GetID();
            Server.items.Add(_id, new Item());

            Server.items[_id].Spawn(_id);

            Server.nextItemTime = Server.itemSpawnDelay;
        }

        public static void EndGame()
        {
            Utilities.Log("Ending game");
            
            Server.gameStarted = false;
            Server.currentMapId = 0;
            ServerSend.EndGame();

            Server.items.Clear();
            Server.projectiles.Clear();

            foreach (Client _client in Server.clients.Values)
            {
                if (_client.player == null) continue;
                _client.player.currentGun = Constants.guns[0];
                _client.player.myAmmo = 0;
                ServerSend.PlayerAmmo(_client.player);
            }
        }

        private static void StartGame()
        {
            Utilities.Log("Starting game");

            Server.gameStarted = true;
            Server.gameTime = Server.gameDuration;
            Server.nextItemTime = Server.itemSpawnDelay;

            // -1 to not include the waiting room, +1 because maps start at 1
            Server.currentMapId = Utilities.RandomInt(Server.maps.Count - 1) + 1;
            ServerSend.StartGame(Server.gameDuration / Constants.TICKS_PER_SEC, Server.currentMapId);

            List<int> possibilites = new List<int>();
            for (int i = 0; i < Server.maps[Server.currentMapId].mapSpawns.Count; i++) possibilites.Add(i);

            foreach (Client _client in Server.clients.Values)
            {
                if (_client.player == null) continue;

                Vector2 _position;
                if (possibilites.Count != 0)
                {
                    int _index = Utilities.RandomInt(possibilites.Count);
                    _position = Server.maps[Server.currentMapId].mapSpawns[possibilites[_index]];
                    possibilites.RemoveAt(_index);
                }
                else
                {
                    _position = Utilities.RandomFreeGoodPosition(_client.player.radius);
                }

                _client.player.isInWaitingRoom = false;
                _client.player.Teleport(_position);
            }
        }

        public static bool StartingConditionsMet()
        {
            if (Server.connectedPlayers < Server.minPlayers) return false;
            if (Server.gameStarted) return false;

            foreach (Client _client in Server.clients.Values)
            {
                if (_client.player == null) continue;
                if (!_client.player.collider.CheckCollision(Server.maps[Server.currentMapId].trigger)) return false;
                if (!_client.player.isInWaitingRoom) return false;
            }

            return true;
        }
    }
}
