using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IEClientLib.Exceptions
{
    public class OpenComException : Exception
    {
        public OpenComException(Exception innEX) : base("无法打开COM",innEX) { }
    }
}
