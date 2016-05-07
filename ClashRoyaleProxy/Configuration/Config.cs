using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ClashRoyaleProxy
{
    class Config
    {
        /// <summary>
        /// Gets a configuration value as string
        /// </summary>
        public static string Get(string key)
        {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);
            return configuration.AppSettings.Settings[key].Value;
        }

        /// <summary>
        /// Sets a configuration value as string
        /// </summary>
        public static void Set(string key, string value)
        {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);
            configuration.AppSettings.Settings[key].Value = value;
            configuration.Save();
            ConfigurationManager.RefreshSection("appSettings");
        }

        // ==============
        // Proxy Settings
        // ==============

        public static int MaxClientConnections
        {
            get
            {
                return Convert.ToInt32(Get("MaxClientConnections"));
            }
        }

        public static string LoggingLevel
        {
            get
            {
                return Get("LoggingLevel");
            }
        }

        public static bool Modding
        {
            get
            {
                return Convert.ToBoolean(Get("Modding"));
            }
        }

        //  ===============
        // Resource Settings
        //  ===============

        public static int Gems
        {
            get
            {
                return Convert.ToInt32(Get("Gems"));
            }
        }

        public static int Gold
        {
            get
            {
                return Convert.ToInt32(Get("Gold"));
            }
        }
    }
}
