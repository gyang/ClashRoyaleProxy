using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ClashRoyaleProxy
{
    class Helper
    {
        /// <summary>
        /// Uses LINQ to return the local IP address
        /// </summary>
        public static IPAddress LocalNetworkIP
        {
            get
            {
                if (!NetworkInterface.GetIsNetworkAvailable())
                    return IPAddress.Parse("127.0.0.1");

                // Establishes an UDP connection to get the proper IP-address
                using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
                {
                    socket.Connect("10.0.2.4", 65530);
                    IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                    return endPoint.Address;
                }
            }
        }

        /// <summary>
        /// Uses LINQ to convert a hexlified string to a byte array
        /// </summary>
        public static byte[] HexToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        /// <summary>
        /// Converts a byte array to a hexlified string
        /// </summary>
        public static string ByteArrayToHex(byte[] ba)
        {
            return BitConverter.ToString(ba).Replace("-", "").ToLower();
        }

        /// <summary>
        /// Returns opened instances
        /// </summary>
        public static int OpenedInstances
        {
            get
            {
                return Process.GetProcessesByName(Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location)).Length;
            }
        }

        /// <summary>
        /// Returns Proxy-Version in the following format:
        /// v1.2.3
        /// </summary>
        public static string AssemblyVersion
        {
            get
            {
                return "v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString().Remove(5);
            }
        }

        /// <summary>
        /// Returns the actual year
        /// </summary>
        public static int ActualYear
        {
            get
            {
                return DateTime.Now.ToLocalTime().Year;
            }
        }

        /// <summary>
        /// Returns the actual time in a localized format
        /// </summary>
        public static string ActualTime
        {
            get
            {
                return DateTime.Now.ToString();
            }
        }
    }
}
