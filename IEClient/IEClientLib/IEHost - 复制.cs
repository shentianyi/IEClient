using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Timers;
using Brilliantech.Framwork.Utils.LogUtil;
using IEClientLib.Enums;
using IEClientLib.Exceptions;
using IEClientLib.Helper;

namespace IEClientLib
{
    /// <summary>
    /// 主机
    /// </summary>
    public class IEHost
    {
        public string Com { get; set; }
        public int BaudRate;
        public Parity Parity;
        // public int DataBits;
        // public StopBits StopBits;
        double TimeOut;
        Timer timer;
        double timeSpan = 0;
        bool slaveResponed = false;
        bool responedTimeOut = false;
        int resendCount = 0;

        private SerialPort sp { get; set; }
        public List<IESlave> Slaves { get; set; }
        private int currentSlaveIndex = 0;
        private CmdType currentCmdType;
        private byte currentSN = 0x00;
        


        public IEHost() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Com">串口号</param>
        /// <param name="BaudRate">波特率</param>
        /// <param name="Parity">校验</param>
        /// <param name="TimeOut">超时，毫秒</param>
        public IEHost(string Com, int BaudRate = 9600, Parity Parity = Parity.None, double TimeOut = 300)
        {
            this.Com = Com;
            this.BaudRate = BaudRate;
            this.Parity = Parity;
            this.TimeOut = TimeOut;

            this.timer = new Timer();
            this.timer.Interval = 1;
            this.timer.Elapsed += Timer_Elapsed;
        }

        public void StartTest()
        {
            if (this.Slaves!=null && this.Slaves.Count > 0)
            {
                for (int i = 0; i < this.Slaves.Count; i++)
                {
                    currentSlaveIndex = i;
                    try
                    {
                        currentCmdType = CmdType.START_TEST;
                        bool r = SendCmd(CmdType.START_TEST, this.Slaves[currentSlaveIndex].Code);
                    }
                    catch (SlaveReponseTimeOutException ex)
                    {
                        LogUtil.Logger.Error(ex.Message + ":" + this.Slaves[currentSlaveIndex].Code);
                    }
                    catch (Exception ex)
                    {
                        LogUtil.Logger.Error(ex.Message + ":" + this.Slaves[currentSlaveIndex].Code);
                    }
                }
                currentSlaveIndex = 0;

            }
            else
            {
                throw new Exception("没有从机");
            }
        }
       

        /// <summary>
        /// 接收到返回，进行处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            slaveResponed = true;
            //throw new NotImplementedException();
        }


        /// <summary>
        /// 发送命令
        /// </summary>
        /// <param name="cmdType"></param>
        /// <param name="slaveCode"></param>
        /// <returns></returns>
        private bool SendCmd(CmdType cmdType, string slaveCode,bool reSend=false)
        {
            LogUtil.Logger.Debug("start send.................");
            if (!IsOpen())
            {
                OpenCom();
            }
            currentCmdType = cmdType;

            byte[] codeBytes = ScaleHelper.HexStringToHexByte(slaveCode);
            if (!reSend)
            {
                GenerateSN();
            }
            byte[] cmd=null;
            switch (cmdType) {
                case CmdType.START_TEST:
                    cmd = new byte[9];
                    cmd[0] = codeBytes[0];
                    cmd[1] = codeBytes[1];
                    cmd[2] = 0xAA; // STX
                    cmd[3] = 0x03; // LEN
                    cmd[4] = 0xC0; // CMD
                    cmd[5] = currentSN;//SN
                    cmd[6] = 0x01;//DATA,任务号
                    byte[] fcc = ScaleHelper.CalculateFcc(new byte[3] { cmd[4], cmd[5], cmd[6] });
                    cmd[7] = fcc[0];
                    cmd[8] = fcc[1];
                    break;
                default:
                    break;
            }

            sp.Write(cmd, 0, cmd.Length);

            timer.Start();
            while (!slaveResponed)
            {
                LogUtil.Logger.Debug("checking.................");

                if (slaveResponed)
                {
                    break;
                }
                if (responedTimeOut)
                {
                    restTimeoutVar();
                    throw new SlaveReponseTimeOutException("从机响应超时");
                }
            }
            restTimeoutVar();
            return slaveResponed;
        }

        private bool IsOpen()
        {
            return this.sp != null && this.sp.IsOpen;
        }

        /// <summary>
        /// 打卡串口
        /// </summary>
        /// <returns></returns>
        private bool OpenCom()
        {
            try
            {
                if (this.sp == null)
                {
                    this.sp = new SerialPort(this.Com, this.BaudRate);
                    this.sp.DataReceived += Sp_DataReceived;
                }
                this.sp.Open();
                return true;
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Error(ex.Message);
                throw ex;
            }
        }
        /// <summary>
        /// 关闭串口
        /// </summary>
        /// <returns></returns>
        private bool Close()
        {
            if (this.sp != null)
            {
                try
                {
                    if (this.sp.IsOpen)
                    {
                        this.sp.Close();
                        LogUtil.Logger.Info("Close Success");
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    LogUtil.Logger.Error("Close Error");
                    LogUtil.Logger.Error(ex.Message);
                    throw ex;
                }
            }
            return false;
        }

        /// <summary>
        /// 计时器，计算是否超时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            timeSpan += timer.Interval;
            if (slaveResponed)
            {
                timeSpan = 0;
                resendCount = 0;
                timer.Stop();
            }
            else
            {
                if (timeSpan > this.TimeOut)
                {
                    LogUtil.Logger.Debug("Time Out......");
                    // 命令超时，重发
                    timer.Stop();
                    timeSpan = 0;
                    resendCount += 1;
                    if (resendCount > 2)
                    {
                        slaveResponed = false;
                        responedTimeOut = true;
                    }
                    else
                    {
                        //TODO 重发命令,SN不变
                        SendCmd(currentCmdType, this.Slaves[currentSlaveIndex].Code, true);
                        timer.Start();
                    }
                }
            }
        }

        private void GenerateSN() {
            if (currentSN == 255)
            {
                currentSN = 0x00;
            }
            currentSN += 1;
        }

        private void restTimeoutVar() {
            timer.Stop();
            timeSpan = 0;
            resendCount = 0;
            slaveResponed = false;
            responedTimeOut = false;
            resendCount = 0;
        }
    }
}
