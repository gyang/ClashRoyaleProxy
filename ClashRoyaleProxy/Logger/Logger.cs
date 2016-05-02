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
        public static void Log(string text, LogType type)
        {
            Console.ForegroundColor = (type == LogType.EXCEPTION || type == LogType.WARNING) ? ConsoleColor.Red : ConsoleColor.Green;
            Console.Write("[" + type + "] ");
            Console.ResetColor();
            Console.WriteLine(text);
            //LogToFile(text, type);
        }

        /// <summary>
        /// Logs passed packet
        /// </summary>
        public static void LogPacket(Packet p)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("[PACKET " + p.ID + "] ");
            Console.ResetColor();
            Console.WriteLine(p.Type);
            LogToHexa(p.DecryptedPayload, p.ID);
        }

        /// <summary>
        /// Logs passed text to a file
        /// </summary>
        public static void LogToFile(string text, LogType type)
        {
            string path = Environment.CurrentDirectory + @"\\Logs\\log_" + DateTime.UtcNow.ToLocalTime().ToString("dd-MM-yyyy") + "." + "log";
            StreamWriter sw = new StreamWriter(new FileStream(path, FileMode.Append));
            sw.WriteLine("[" + DateTime.UtcNow.ToLocalTime().ToString("hh-mm-ss") + "-" + type + "] " + text);
            sw.Close();
        }

        public static void LogToHexa(byte[] p, int i)
        {
            if (!Directory.Exists(@"Packets\" + i))
                Directory.CreateDirectory(@"Packets\" + i);
            File.WriteAllText(@"Packets\" + i + @"\hexa.bin", BitConverter.ToString(p).Replace("-", string.Empty));
            File.WriteAllText(@"Packets\" + i + @"\ascii.bin", Encoding.ASCII.GetString(p));
        }
    }
}
