﻿using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Timers;
using ClearInsight;
using IEClientLib;
using IEClientLib.Enums;
using IEClientLib.Helper;

namespace TestCon
{
    class Program
    {
        static int i = 0;
        static void Main(string[] args)
        {
            string url = "http://192.168.1.11:3000";

            //IEHost host = new IEHost("COM3", 9600, Parity.None, 5000);
            //List<IESlave> slaves = new List<IESlave>();
            //for (var i = 1; i < 100; i++)
            //{
            //    IESlave s = new IESlave()
            //    {
            //        Index = i,
            //        Code = ScaleHelper.DecimalToHexString(i)
            //    };
            //    s.StatusChanged += new IESlave.StatusChangedEventHandler(Status_QtyChanged);
            //    s.TimeTicked += new IESlave.TimeTickedEventHandler(S_TimeTicked);
            //    slaves.Add(s);
            //}
            //host.Slaves = slaves;
            //host.PollData();

            //Console.WriteLine(ScaleHelper.HexByteToDecimal(0xff));
            //Console.WriteLine(ScaleHelper.HexBytesToDecimal(new byte[3] { 0xFF,0x00,0xFF}));
            ClearInsightAPI ci = new ClearInsightAPI(url);
            var u = ci.UserLogin("", "");
            if (u.result)
            {
                Console.WriteLine("hhhhh");
            }
            else
            {
                Console.WriteLine("nnnnnn");
            }
            Console.Read();
        }

        private static void S_TimeTicked(IESlave slave, IEData data)
        {
            Console.WriteLine("=======================:" + slave.Code + " :" + data.Time);
        }

        private static void Status_QtyChanged(IESlave slave)
        {
            Console.WriteLine("================================status changed:" + slave.Code + "=======" + slave.Status);
        }
        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            i += 1;
            Console.WriteLine(i);
        }
    }
}
