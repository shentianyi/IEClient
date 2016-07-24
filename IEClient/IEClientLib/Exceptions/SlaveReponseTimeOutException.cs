using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IEClientLib.Exceptions
{
    public class SlaveReponseTimeOutException:Exception
    {
        public SlaveReponseTimeOutException() { }
        public SlaveReponseTimeOutException(string exMsg) : base(exMsg) { }
    }
}
