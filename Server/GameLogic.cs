using System;
using System.Numerics;

namespace GameServer
{
    class GameLogic
    {
        public static void Update()
        {
            // Logic
            if (StartingConditionsMet())
            {
                Server.gameStarted = true;
                ServerSend.StartGame();

                foreach (Client _client in Server.clients.Values)
                {
                    if (_client.player == null) continue;
                    Vector2 _position = Utilities.RandomFreeCirclePosition(_client.player.radius);
                    _client.player.Teleport(_position);
                }
            }
            if (Server.gameStarted) Server.gameTime -= 1;
            if (Server.gameTime == 0 &&
                Server.gameStarted)
            {
                Console.WriteLine("Game ended");
                Server.gameStarted = false;
                ServerSend.EndGame();

                foreach (Client _client in Server.clients.Values)
                {
                    if (_client.player == null) continue;
                    _client.player.Teleport(Constants.WAITING_ROOM_SPAWN);
                }
            }

            // Clients
            foreach (Client _client in Server.clients.Values)
            {
                if (_client.player == null) continue;
                _client.player.Update();
            }

            // Items
            if (Server.gameStarted)
            {
                if (Server.nextItemTime <= 0 &&
                    Server.items.Count < Server.maxItems)
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
                Server.nextItemTime -= 1;
            }

            // Projectiles
            foreach (Projectile projectile in Server.projectiles.Values)
            {
                projectile.Update();
            }

            ThreadManager.UpdateMain();
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
