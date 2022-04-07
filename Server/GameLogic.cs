using System;
using System.Numerics;

namespace GameServer
{
    class GameLogic
    {
        public static void Update()
        {
            if (StartingConditionsMet())
            {
                Server.gameStarted = true;
                ServerSend.StartGame();

                // TODO: TP Players to spawn points
                foreach (Client _client in Server.clients.Values)
                {
                    if (_client.player == null) continue;
                    _client.player.Teleport(new Vector2(0, 0));
                }
            }

            // Clients
            foreach (Client _client in Server.clients.Values)
            {
                if (_client.player == null) continue;
                _client.player.Update();
            }

            // Items
            if (Server.nextItemTime <= 0 &&
                Server.items.Count < Server.maxItems &&
                Server.gameStarted == true)
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
            if (Server.gameStarted == true)
            {
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
