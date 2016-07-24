using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IEClientLib.Enums
{
    /// <summary>
    /// 从机的状态
    /// </summary>
    public enum SlaveStatus
    {
        /// <summary>
        /// 未在线
        /// </summary>
        OFF_LINE = 100,

        /// <summary>
        /// 刚获取到任务后的状态, 可以开始测试
        /// </summary>
        OK_TO_TEST = 200,

        /// <summary>
        /// 刚获取到任务后的状态, 不可以开始测试
        /// </summary>
        NOK_TO_TEST = 201,

        /// <summary>
        /// 报文解析失败
        /// </summary>
        MSG_PARSE_ERROR = 300,

        /// <summary>
        /// 任务ID不一致
        /// </summary>
        ID_NOT_MATCH = 400,

        /// <summary>
        /// 测试中
        /// </summary>
        ON_TESTING = 500,

        /// <summary>
        /// 未在测试
        /// </summary>
        OUT_TEST = 600
    }
}