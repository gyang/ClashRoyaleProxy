using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Timers;

namespace ClashRoyaleProxy
{
    class Proxy
    {
        private static List<Client> ClientPool = new List<Client>();
        private const int PORT = 9339;
        private const int MAX_CONNECTIONS = 100;

        /// <summary>
        /// Initializes the proxy console interface
        /// </summary>
        public static void InitUI()
        {
            // Intilize the UI
            Console.SetCursorPosition(0, 0);
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.Title = "Clash Royale Proxy " + Helper.AssemblyVersion + " | © " + Helper.ActualYear;

            // Start a timer refreshing the console title
            var titleRefreshTimer = new System.Timers.Timer();
            titleRefreshTimer.Interval = 1000;      
            titleRefreshTimer.Elapsed += new ElapsedEventHandler((s, e) =>
            {
                Console.Title = "Clash Royale Proxy " + Helper.AssemblyVersion + " | © " + Helper.ActualYear + " | " + Helper.ActualTime;
            });
            titleRefreshTimer.Start();
        }

        /// <summary>
        /// Starts the proxy
        /// </summary>
        public static void Start()
        {
            if (!Directory.Exists("Logs"))
                Directory.CreateDirectory("Logs");
            if(!Directory.Exists("Packets"))
                Directory.CreateDirectory("Packets");

            // Bind a new socket to the local EP
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, PORT);
            Socket clientListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientListener.Bind(endPoint);
            clientListener.Listen(MAX_CONNECTIONS);
            Logger.Log("Successfully bound socket!");
            Logger.Log("Hint: ClashRoyaleProxy v1.2.0 introduces a new config file!");
            Logger.Log("Take a look at ClashRoyaleProxy.exe.config!");
            Logger.Log("Please connect to " + Helper.LocalNetworkIP + " if you're ready.");
            Logger.Log("Listening..");

            while (true)
            {
                Socket clientSocket = clientListener.Accept();
                if((ClientPool.ToArray().Length + 1) > Config.MaxClientConnections)
                {
                    Logger.Log("Sorry, only " + Config.MaxClientConnections + " clients may connect.");
                    return;
                }

                // Client connected, let's enqueue him!
                Client client = new Client(clientSocket);
                Logger.Log("Remote connection from client #" + (ClientPool.ToArray().Length + 1) + " (" + client.ClientRemoteAdr + "), enqueuing..");
                ClientPool.Add(client);
                client.Enqueue();
            }
        }

        /// <summary>
        /// Stops the proxy
        /// </summary>
        public static void Stop()
        {
            for (int i = 0; i < ClientPool.Count; i++)
            {
                ClientPool[i].Dequeue();
            }
            ClientPool.Clear();
        }

        /// <summary>
        /// Returns the client pool
        /// </summary>
        public static List<Client> GetClientPool()
        {
            return ClientPool;
        }
    }
}
