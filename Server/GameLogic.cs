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
            // TODO: Method to add value to first integer key.
            int _index = 0;
            while (true)
            {
                if (Server.items.TryAdd(_index, new Item()))
                {
                    Server.items[_index].Spawn(_index);
                    break;
                }
                _index += 1;
            }

            Server.nextItemTime = Server.itemSpawnDelay;
        }

        private static void EndGame()
        {
            Console.WriteLine("Game ended");
            Server.gameStarted = false;
            Server.currentMap = Server.maps[0];
            ServerSend.EndGame();

            Server.items.Clear();
            Server.projectiles.Clear();

            foreach (Client _client in Server.clients.Values)
            {
                if (_client.player == null) continue;
                _client.player.currentGun = Constants.guns[0];
                _client.player.myAmmo = 10;
                ServerSend.PlayerAmmo(_client.player);

                // FIXME
                _client.player.Teleport(Constants.WAITING_ROOM_SPAWN);
            }
        }

        private static void StartGame()
        {
            Server.gameStarted = true;
            Server.gameTime = Server.gameDuration;
            Server.nextItemTime = Server.itemSpawnDelay;

            // -1 to not include the waiting room, +1 because maps start at 1
            int randomMapId = Utilities.RandomInt(Server.maps.Count - 1) + 1;
            Server.currentMap = Server.maps[randomMapId];
            ServerSend.StartGame(Server.gameDuration / Constants.TICKS_PER_SEC, randomMapId);

            List<int> possibilites = new List<int>();
            for(int i = 0; i < Server.currentMap.mapSpawns.Count; i++)  possibilites.Add(i);

            foreach (Client _client in Server.clients.Values)
            {
                if (_client.player == null) continue;

                Vector2 _position;
                if (possibilites.Count != 0)
                {
                    int _index = Utilities.RandomInt(possibilites.Count);
                    _position = Server.currentMap.mapSpawns[possibilites[_index]];
                    possibilites.RemoveAt(_index);
                } else {
                    _position = Utilities.RandomFreeCirclePositionInMap(_client.player.radius);
                }

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

                if (!_client.player.collider.CheckCollision(Server.currentMap.trigger))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
