using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace GameServer
{
    class Server
    {
        // Networking
        public static int MaxPlayers { get; private set; }
        public static int Port { get; private set; }
        public static Dictionary<int, Client> clients = new Dictionary<int, Client>();
        public delegate void PacketHandler(int _fromClient, Packet _packet);
        public static Dictionary<int, PacketHandler> packetHandlers;
        private static TcpListener tcpListener;
        private static UdpClient udpListener;

        // Logic
        public static int connectedPlayers = 0;
        public static int minPlayers = 1; // Minimum amount of players needed to start the game
        public static bool gameStarted = false;

        public static int gameDuration = 120  * Constants.TICKS_PER_SEC; // Duration of a game, in ticks.
        public static int gameTime = gameDuration; // Time left of the game, in ticks.

        // Scene
        public static string scenesPath = "Scenes";
        public static int currentMapId;
        public static Dictionary<int, Map> maps = new Dictionary<int, Map>();

        // Items
        public static Dictionary<int, Item> items = new Dictionary<int, Item>();
        public static int maxItems = 5;
        public static float itemSpawnDelay = (float) 5 * Constants.TICKS_PER_SEC; // Delay between items, in ticks.
        public static float nextItemTime = itemSpawnDelay;

        // Projectiles
        public static Dictionary<int, Projectile> projectiles = new Dictionary<int, Projectile>();


        public static void Start(int _maxPlayers, int _port)
        {
            MaxPlayers = _maxPlayers;
            Port = _port;

            Console.WriteLine("Starting server...");
            InitializeServerData();

            tcpListener = new TcpListener(IPAddress.Any, Port);
            tcpListener.Start();
            tcpListener.BeginAcceptTcpClient(TCPConnectCallback, null);

            udpListener = new UdpClient(Port);
            udpListener.BeginReceive(UDPReceiveCallback, null);

            Console.WriteLine($"Server started on port {Port}.");
        }

        private static void TCPConnectCallback(IAsyncResult _result)
        {
            TcpClient _client = tcpListener.EndAcceptTcpClient(_result);
            tcpListener.BeginAcceptTcpClient(TCPConnectCallback, null);
            Console.WriteLine($"Incoming connection from {_client.Client.RemoteEndPoint}...");

            for (int i = 1; i <= MaxPlayers; i++)
            {
                if (clients[i].tcp.socket == null)
                {
                    clients[i].tcp.Connect(_client);
                    return;
                }
            }

            Console.WriteLine($"{_client.Client.RemoteEndPoint} failed to connect: Server full!");
        }

        private static void UDPReceiveCallback(IAsyncResult _result)
        {
            try
            {
                IPEndPoint _clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] _data = udpListener.EndReceive(_result, ref _clientEndPoint);
                udpListener.BeginReceive(UDPReceiveCallback, null);

                if (_data.Length < 4)
                {
                    return;
                }

                using (Packet _packet = new Packet(_data))
                {
                    int _clientId = _packet.ReadInt();

                    if (_clientId == 0)
                    {
                        return;
                    }

                    if (clients[_clientId].udp.endPoint == null)
                    {
                        clients[_clientId].udp.Connect(_clientEndPoint);
                        return;
                    }

                    if (clients[_clientId].udp.endPoint.ToString() == _clientEndPoint.ToString())
                    {
                        clients[_clientId].udp.HandleData(_packet);
                    }
                }
            }
            catch (Exception _ex)
            {
                Console.WriteLine($"Error receiving UDP data: {_ex}");
            }
        }

        public static void SendUDPData(IPEndPoint _clientEndPoint, Packet _packet)
        {
            try
            {
                if (_clientEndPoint != null)
                {
                    udpListener.BeginSend(_packet.ToArray(), _packet.Length(), _clientEndPoint, null, null);
                }
            }
            catch (Exception _ex)
            {
                Console.WriteLine($"Error sending data to {_clientEndPoint} via UDP: {_ex}");
            }
        }

        private static void InitializeServerData()
        {
            for (int i = 1; i <= MaxPlayers; i++)
            {
                clients.Add(i, new Client(i));
            }

            Console.WriteLine("Importing maps...");

            string mapsPath = Path.Join(scenesPath, "Maps");
            foreach (string path in Directory.GetFiles(mapsPath)) {
                int index = int.Parse(path.Substring(path.Length - 7, 1));
                maps[index] = new Map(path);
            }
            string waitingRoomMapPath = Path.Join(scenesPath, "WaitingRoom.unity");
            maps[0] = new Map(waitingRoomMapPath);
            currentMapId = 0;
            // waitingRoomMap = new Map(waitingRoomMapPath);
      
            Console.WriteLine("Imported maps.");

            packetHandlers = new Dictionary<int, PacketHandler>()
            {
                { (int)ClientPackets.welcomeReceived, ServerHandle.WelcomeReceived },
                { (int)ClientPackets.playerName, ServerHandle.PlayerName },
                { (int)ClientPackets.playerMovement,  ServerHandle.PlayerMovement},
                { (int)ClientPackets.playerShoot,  ServerHandle.PlayerShoot},
                { (int)ClientPackets.playerEndGame,  ServerHandle.PlayerEndGame},
            };
            Console.WriteLine("Initialized packets.");
        }
    }
}
