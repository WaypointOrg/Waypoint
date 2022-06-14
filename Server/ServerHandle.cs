using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer
{
    class ServerHandle
    {
        public static void WelcomeReceived(int _fromClient, Packet _packet)
        {
            int _clientIdCheck = _packet.ReadInt();
            string _username = _packet.ReadString();

            Console.WriteLine($"{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully and is now player {_fromClient}.");
            if (_fromClient != _clientIdCheck)
            {
                Console.WriteLine($"Player \"{_username}\" (ID: {_fromClient}) has assumed the wrong client ID ({_clientIdCheck})!");
            }
            Server.clients[_fromClient].SendIntoGame(_username);
        }

        public static void PlayerName(int _fromClient, Packet _packet)
        {
            string _name = _packet.ReadString();

            Server.clients[_fromClient].player.username = _name;

            ServerSend.SetName(Server.clients[_fromClient].player);
        }

        public static void PlayerMovement(int _fromClient, Packet _packet)
        {
            bool[] _inputs = new bool[_packet.ReadInt()];
            for (int i = 0; i < _inputs.Length; i++)
            {
                _inputs[i] = _packet.ReadBool();
            }
            float _rotation = _packet.ReadFloat();

            // Exeption can happen here: 
            // Exception has occurred: CLR/System.NullReferenceException
            // An exception of type 'System.NullReferenceException' occurred in GameServer.dll but was not handled in user code: 'Object reference not set to an instance of an object.'
            //    at GameServer.ServerHandle.PlayerMovement(Int32 _fromClient, Packet _packet) in /home/daniel/Waypoint/Server/ServerHandle.cs:line 40
            //    at GameServer.Client.UDP.<>c__DisplayClass5_0.<HandleData>b__0() in /home/daniel/Waypoint/Server/Client.cs:line 180
            //    at GameServer.ThreadManager.UpdateMain() in /home/daniel/Waypoint/Server/ThreadManager.cs:line 45
            //    at GameServer.GameLogic.Update() in /home/daniel/Waypoint/Server/GameLogic.cs:line 51
            //    at GameServer.Program.MainThread() in /home/daniel/Waypoint/Server/Program.cs:line 30
            //    at System.Threading.ThreadHelper.ThreadStart_Context(Object state)
            //    at System.Threading.ExecutionContext.RunInternal(ExecutionContext executionContext, ContextCallback callback, Object state)

            // probably should check if player == null
            Server.clients[_fromClient].player.SetInput(_inputs, _rotation);
        }

        public static void PlayerShoot(int _fromClient, Packet _packet)
        {
            Server.clients[_fromClient].player.Shoot();
        }
    }
}
