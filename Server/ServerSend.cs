using System.Numerics;

namespace GameServer
{
    class ServerSend
    {
        private static void SendTCPData(int _toClient, Packet _packet)
        {
            _packet.WriteLength();
            Server.clients[_toClient].tcp.SendData(_packet);
        }

        private static void SendUDPData(int _toClient, Packet _packet)
        {
            _packet.WriteLength();
            Server.clients[_toClient].udp.SendData(_packet);
        }

        private static void SendTCPDataToAll(Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                Server.clients[i].tcp.SendData(_packet);
            }
        }
        private static void SendTCPDataToAll(int _exceptClient, Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                if (i != _exceptClient)
                {
                    Server.clients[i].tcp.SendData(_packet);
                }
            }
        }

        private static void SendUDPDataToAll(Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                Server.clients[i].udp.SendData(_packet);
            }
        }
        private static void SendUDPDataToAll(int _exceptClient, Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                if (i != _exceptClient)
                {
                    Server.clients[i].udp.SendData(_packet);
                }
            }
        }

        #region Packets
        public static void Welcome(int _toClient, string _msg)
        {
            using (Packet _packet = new Packet((int)ServerPackets.welcome))
            {
                _packet.Write(_msg);
                _packet.Write(_toClient);

                SendTCPData(_toClient, _packet);
            }
        }

        public static void SpawnPlayer(int _toClient, Player _player)
        {
            using (Packet _packet = new Packet((int)ServerPackets.spawnPlayer))
            {
                _packet.Write(_player.id);
                _packet.Write(_player.username);
                _packet.Write(_player.position);
                _packet.Write(_player.rotation);

                SendTCPData(_toClient, _packet);
            }
        }

        public static void PlayerPosition(Player _player)
        {
            using (Packet _packet = new Packet((int)ServerPackets.playerPosition))
            {
                _packet.Write(_player.id);
                _packet.Write(_player.position);

                SendUDPDataToAll(_packet);
            }
        }

        public static void PlayerRotation(Player _player)
        {
            using (Packet _packet = new Packet((int)ServerPackets.playerRotation))
            {
                _packet.Write(_player.id);
                _packet.Write(_player.rotation);

                SendUDPDataToAll(_player.id, _packet);
            }
        }

        public static void ItemSpawned(Item _item)
        {
            using (Packet _packet = new Packet((int)ServerPackets.itemSpawned))
            {
                _packet.Write(_item.itemId);
                _packet.Write(_item.position);
                _packet.Write((int) _item.type);

                SendTCPDataToAll(_packet);
            }
        }

        public static void ItemPickedUp(Item _item, Player _player)
        {
            using (Packet _packet = new Packet((int)ServerPackets.itemPickedUp))
            {
                _packet.Write(_item.itemId);
                _packet.Write(_player.id);

                SendTCPDataToAll(_packet);
            }
        }

        public static void ProjectileSpawned(Projectile _projectile)
        {
            using (Packet _packet = new Packet((int)ServerPackets.projectileSpawned))
            {
                _packet.Write(_projectile.projectileId);
                _packet.Write(_projectile.position);
                _packet.Write((int) _projectile.type);

                SendTCPDataToAll(_packet);
            }
        }

        public static void ProjectilePosition(Projectile _projectile)
        {
            using (Packet _packet = new Packet((int)ServerPackets.projectilePosition))
            {
                _packet.Write(_projectile.projectileId);
                _packet.Write(_projectile.position);

                SendUDPDataToAll(_packet);
            }
        }

        public static void ProjectileDestroy(Projectile _projectile)
        {
            using (Packet _packet = new Packet((int)ServerPackets.projectileDestroyed))
            {
                _packet.Write(_projectile.projectileId);

                SendTCPDataToAll(_packet);
            }
        }

        #endregion
    }
}
