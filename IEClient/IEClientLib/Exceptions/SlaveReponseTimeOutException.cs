using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IEClientLib.Exceptions
{
    public class SlaveReponseTimeOutException:Exception
    { 
        public SlaveReponseTimeOutException(Exception innEX) : base("从机响应超时",innEX) { }
    }
}
