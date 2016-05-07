using System;
using System.IO;
using System.Text;

namespace ClashRoyaleProxy
{
    class Logger
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

                string path = Environment.CurrentDirectory + @"\\Logs\\" + DateTime.UtcNow.ToLocalTime().ToString("dd-MM-yyyy") + ".log";
                using (FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (StreamWriter StreamWriter = new StreamWriter(fs))
                    {
                        StreamWriter.WriteLine("[" + DateTime.UtcNow.ToLocalTime().ToString("hh-mm-ss") + "-" + type + "] " + text);
                        StreamWriter.Close();
                    }
                }

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
                Logger.Log(p.ID + " : " + p.Type, LogType.PACKET);
                Console.ResetColor();
                LogPacketToHexFile(p);
            }
        }

        /// <summary>
        /// Logs passed packet to a hex file
        /// </summary>
        public static void LogPacketToHexFile(Packet packet)
        {
            if (!Directory.Exists(@"Packets\\" + packet.ID.ToString()))
                Directory.CreateDirectory(@"Packets\\" + packet.ID.ToString());
            File.WriteAllText(@"Packets\\" + packet.ID + @"\\hex.txt", BitConverter.ToString(packet.DecryptedPayload).Replace("-", " "));
            File.WriteAllText(@"Packets\\" + packet.ID + @"\\ascii.bin", Encoding.UTF8.GetString(packet.DecryptedPayload));
        }
    }
}
