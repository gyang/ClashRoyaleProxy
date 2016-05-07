using System;
using System.Threading;

namespace ClashRoyaleProxy
{
    class Program
    {
        /// <summary>
        /// "Kills" the entire program
        /// </summary>
        public static void Exit(int exitCode = 0)
        {
            Environment.Exit(exitCode);
        }

        /// <summary>
        /// Entry point for the proxy
        /// </summary>
        public static void Main(string[] args)
        {
            // UI
            Proxy.InitUI();

            // Check whether the proxy runs more than once
            if (Helper.OpenedInstances > 1)
            {
                Logger.Log("You seem to run this proxy more than once.", LogType.WARNING);
                Logger.Log("Aborting..", LogType.WARNING);
                Thread.Sleep(1350);
                Program.Exit(-1);
            }

            // Proxy
            Proxy.Start();
        }
    }
}