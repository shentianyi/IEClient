using Brilliantech.Framwork.Utils.ConfigUtil;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;

namespace IEClient.Config
{
    public class BaseConfig
    {
        private static ConfigUtil config;

        private static string server;
        private static string com;
        private static int baundRate;
        private static Parity parity;
        private static int timeOut;
        private static string cycleTimeKpiCode;



        static BaseConfig()
        {
            config = new ConfigUtil("BASE","Config/base.ini");

            server = config.Get("Server");
            com = config.Get("Com");
            baundRate = int.Parse(config.Get("BaundRate"));
            parity = (Parity)int.Parse(config.Get("Parity"));
            timeOut = int.Parse(config.Get("TimeOut"));
            cycleTimeKpiCode = config.Get("CycleTimeKpiCode");

        }
        public static string Server
        {
            get
            {
                return server;
            }

            set
            {
                server = value;
                config.Set("Server", value);
                config.Save();
            }
        }

        public static string Com
        {
            get
            {
                return com;
            }

            set
            {
                com = value;
                config.Set("Com", value);
                config.Save();
            }
        }

        public static int BaundRate
        {
            get
            {
                return baundRate;
            }

            set
            {
                baundRate = value;

                config.Set("BaundRate", value);
                config.Save();
            }
        }

        public static Parity Parity
        {
            get
            {
                return parity;
            }

            set
            {
                parity = value;
                config.Set("Parity", (int)value);
                config.Save();
            }
        }

        public static int TimeOut
        {
            get
            {
                return timeOut;
            }

            set
            {
                timeOut = value;
                config.Set("timeOut", value);
                config.Save();
            }
        }

        public static string CycleTimeKpiCode {
            get { return cycleTimeKpiCode; }
        }
    }
}
