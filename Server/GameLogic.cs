using System;
using System.Numerics;

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
            ServerSend.EndGame();

            Server.items.Clear();

            foreach (Client _client in Server.clients.Values)
            {
                if (_client.player == null) continue;
                _client.player.trajectory = Constants.Trajectories.Straight;
                _client.player.Teleport(Constants.WAITING_ROOM_SPAWN);
            }
        }

        private static void StartGame()
        {
            Server.gameStarted = true;
            Server.gameTime = Server.gameDuration;
            ServerSend.StartGame(Server.gameDuration / Constants.TICKS_PER_SEC);

            foreach (Client _client in Server.clients.Values)
            {
                if (_client.player == null) continue;
                Vector2 _position = Utilities.RandomFreeCirclePosition(_client.player.radius);
                _client.player.Teleport(_position);
            }
        }

        public static bool StartingConditionsMet()
        {
            if (Server.connectedPlayers < Server.minPlayers) return false;
            foreach (Client _client in Server.clients.Values)
            {
                if (_client.player == null) continue;

                if (!_client.player.collider.CheckCollision(Server.scene.trigger))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
