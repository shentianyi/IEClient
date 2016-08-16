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
        private static int outClockingMax;
        private static int onClockingMax;
        private static int? minimunValue;
        private static int? maxmunValue;

        static BaseConfig()
        {
            config = new ConfigUtil("BASE","Config/base.ini");

            server = config.Get("Server");
            com = config.Get("Com");
            baundRate = int.Parse(config.Get("BaundRate"));
            parity = (Parity)int.Parse(config.Get("Parity"));
            timeOut = int.Parse(config.Get("TimeOut"));
            cycleTimeKpiCode = config.Get("CycleTimeKpiCode");
            outClockingMax = int.Parse(config.Get("OutClockingMax"));
            onClockingMax = int.Parse(config.Get("OnClockingMax"));
            minimunValue = int.Parse(config.Get("MinimunValue"));
            maxmunValue = int.Parse(config.Get("MaxmunValue"));
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
                config.Set("TimeOut", value);
                config.Save();
            }
        }

        public static string CycleTimeKpiCode {
            get { return cycleTimeKpiCode; }
        }

        public static int OutClockingMax
        {
            get
            {
                return outClockingMax;
            }

            set
            {
                outClockingMax = value;
                config.Set("OutClockingMax", value);
                config.Save();
            }
        }

        public static int OnClockingMax
        {
            get
            {
                return onClockingMax;
            }

            set
            {
                onClockingMax = value;
                config.Set("OnClockingMax", value);
                config.Save();
            }
        }
        public static int? MinimunValue
        {
            get
            {
                return minimunValue;
            }

            set
            {
                minimunValue = value;
                config.Set("MinimunValue", value);
                config.Save();
            }
        }
        public static int? MaxmunValue
        {
            get
            {
                return maxmunValue;
            }

            set
            {
                maxmunValue = value;
                config.Set("MaxmunValue", value);
                config.Save();
            }
        }
    }
}
