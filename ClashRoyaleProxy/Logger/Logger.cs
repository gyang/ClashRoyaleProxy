using System;
using System.IO;
using System.Text;

namespace ClashRoyaleProxy
{
using System.Diagnostics;
    {
        /// <summary>
        /// Logs passed text
        /// </summary>
        public static void Log(string text, LogType type = LogType.INFO)
        {
            if (Config.LoggingLevel == "default")
            {
                switch (type)
                {
                    case LogType.INFO:
                        Console.ForegroundColor = ConsoleColor.Green;
                        break;
                    case LogType.WARNING:
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        break;
                    case LogType.EXCEPTION:
                        Console.ForegroundColor = ConsoleColor.Red;
                        break;
                    case LogType.PACKET:
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        break;
                    case LogType.PACKET_DATA:
                        Console.ForegroundColor = ConsoleColor.DarkMagenta;
                        break;
                }

                Console.Write("[" + type + "] ");
                Console.ResetColor();
                Console.WriteLine(text);

                WriteEntry(text, type.ToString());
            }
        }

        /// <summary>
        /// Logs passed packet
        /// </summary>
        public static void LogPacket(Packet p)
        {
            if (Config.LoggingLevel == "default")
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Logger.Log(p.ID + " : " + p.Type + ", hex : " + Encoding.UTF8.GetString(p.DecryptedPayload),
                    LogType.PACKET);
                Console.ResetColor();
            }
        }

        private static void WriteEntry(string message, string type)
        {
            Trace.WriteLine(
                string.Format("{0},{1},{2}",
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    type,
                    message));
        }
    }
}
