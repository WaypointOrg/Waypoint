using System;
using System.Numerics;

namespace GameServer
{
    class GameLogic
    {
        public static void Update()
        {
            // Clients
            foreach (Client _client in Server.clients.Values)
            {
                if (_client.player != null)
                {
                    _client.player.Update();
                }
            }

            // Items
            if (Server.nextItemTime <= 0 &&
                Server.items.Count < Server.maxItems &&
                Server.connectedPlayers > 0)
            {
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
            
            ThreadManager.UpdateMain();
        }
    }
}
