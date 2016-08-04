using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;

namespace TestCon
{
   public class SerialPortReadTimeout
    {
        SerialPort sp = new SerialPort("COM25");
        public void Test() {
            
            sp.ReadTimeout = 1000;
            sp.DataReceived += Sp_DataReceived;
            sp.Open();
           Console.WriteLine( sp.Read(new byte[0],0,0));
        }

        private void Sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            
        }
    }
}
