using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [Description("离线")]
        OFF = 100,

        /// <summary>
        /// 刚获取到任务后的状态, 可以开始测试
        /// </summary>
        [Description("可以测试")]
        OK_TO_TEST = 200,

        /// <summary>
        /// 刚获取到任务后的状态, 不可以开始测试
        /// </summary>
        [Description("不可测试")]
        NOK_TO_TEST = 201,

        /// <summary>
        /// 报文解析失败
        /// </summary>
        [Description("报文错误")]
        MSG_PARSE_ERROR = 300,

        /// <summary>
        /// 任务ID不一致
        /// </summary>
        [Description("任务不一致")]
        ID_NOT_MATCH = 400,

        /// <summary>
        /// 计时中
        /// </summary>
        [Description("计时中")]
        ON_CLOCKING = 500,

        /// <summary>
        /// 在线，但未在计时
        /// </summary>
        [Description("未计时")]
        OUT_CLOCKING = 600
    }
}