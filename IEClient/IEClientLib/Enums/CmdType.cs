using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IEClientLib.Enums
{
    /// <summary>
    /// 命令类型
    /// </summary>
    public enum CmdType
    {
        START_TEST,
        STOP_TEST,
        POLL_DATA,
        TRANS_DATA,

        ACK,
        NAK
    }
}
