using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
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
        /// <summary>
        /// 串口参数
        /// </summary>
        private SerialPort sp { get; set; }
        private string Com { get; set; }
        private int BaudRate;
        private Parity Parity;

        /// <summary>
        /// 接受超时及重发
        /// </summary>
        private int timeout = 300;
        private int resendCount = 0;

        /// <summary>
        /// 从机及当前的变量
        /// </summary>
        public List<IESlave> Slaves { get; set; }//从机列表
        private int currentSlaveIndex = 0;//从机序号
        private CmdType currentCmdType; //命令类型
        private byte currentSN = 0x00; //数据帧编号0-255
        private byte currentTaskId = 0x01; //任务ID
        private bool started = false;

        /// <summary>
        /// pulldata定时器
        /// </summary>
        private System.Timers.Timer pollDataTimer;



        public IEHost() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Com">串口号</param>
        /// <param name="BaudRate">波特率</param>
        /// <param name="Parity">校验</param>
        /// <param name="TimeOut">从机反馈超时，毫秒</param>
        public IEHost(string Com, int BaudRate = 9600, Parity Parity = Parity.None, int TimeOut = 300)
        {
            this.Com = Com;
            this.BaudRate = BaudRate;
            this.Parity = Parity;

            this.timeout = TimeOut;

            this.pollDataTimer = new System.Timers.Timer();
            this.pollDataTimer.Interval = 3000;
            this.pollDataTimer.Elapsed += PollDataTimer_Elapsed;
        }

        
        /// <summary>
        /// 开始测试
        /// </summary>
        public void StartTest()
        {
            StartOrStopTest(CmdType.START_TEST);
            this.started = true;
            this.pollDataTimer.Enabled = true;
            
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        public void PollData()
        {
            DoPollData();
        }

        /// <summary>
        /// 停止测试
        /// </summary>
        public void StopTest()
        {
            started = false;
            this.pollDataTimer.Stop();
            this.pollDataTimer.Enabled = false;
            StartOrStopTest(CmdType.STOP_TEST);
        }

        /// <summary>
        /// 开始或停止测试
        /// </summary>
        /// <param name="cmdType"></param>
        private void StartOrStopTest(CmdType cmdType)
        {
            this.CheckSlaves();
            for (int i = 0; i < this.Slaves.Count; i++)
            {
                currentSlaveIndex = i;
                try
                {
                    currentCmdType = cmdType;
                    bool r = SendCmd(cmdType, this.Slaves[currentSlaveIndex].Code);
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


        /// <summary>
        /// 发送命令
        /// </summary>
        /// <param name="cmdType">命令类型</param>
        /// <param name="slaveCode">从机编码</param>
        /// <param name="reSend">是否是重发</param>
        /// <param name="needAK">是否需要从机确认</param>
        /// <returns></returns>
        private bool SendCmd(CmdType cmdType, string slaveCode,bool reSend=false,bool needAK=true)
        {
            Thread.Sleep(10);

            if (string.IsNullOrWhiteSpace(slaveCode)) {
                return false;
            }
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
            byte[] cmd = new byte[9];
            byte[] fcc = new byte[2];

            switch (cmdType)
            {
                case CmdType.START_TEST:
                case CmdType.STOP_TEST:
                    cmd[0] = codeBytes[0];
                    cmd[1] = codeBytes[1];
                    cmd[2] = 0xAA; // STX
                    cmd[3] = 0x03; // LEN
                    cmd[4] = (byte)(cmdType == CmdType.START_TEST ? 0xC0 : 0xC1); // CMD
                    cmd[5] = currentSN;//SN
                    cmd[6] = currentTaskId;//DATA,任务号
                    fcc = ScaleHelper.CalculateFcc(new byte[3] { cmd[4], cmd[5], cmd[6] });
                    cmd[7] = fcc[0];
                    cmd[8] = fcc[1];
                    break;
                case CmdType.POLL_DATA:
                    cmd[0] = codeBytes[0];
                    cmd[1] = codeBytes[1];
                    cmd[2] = 0xAA; // STX
                    cmd[3] = 0x03; // LEN
                    cmd[4] = 0xC2; //指令
                    cmd[5] = currentSN;
                    cmd[6] = currentTaskId;
                    fcc = ScaleHelper.CalculateFcc(new byte[3] { cmd[4], cmd[5], cmd[6] });
                    cmd[7] = fcc[0];
                    cmd[8] = fcc[1];
                    break;
                case CmdType.ACK:
                     cmd = new byte[8];
                    cmd[0] = codeBytes[0];
                    cmd[1] = codeBytes[1];
                    cmd[2] = 0xAA; // STX
                    cmd[3] = 0x02; // LEN
                    cmd[4] = 0xB0; //指令
                    cmd[5] = currentSN;
                    fcc = ScaleHelper.CalculateFcc(new byte[3] { cmd[4], cmd[5], cmd[6] });
                    cmd[6] = fcc[0];
                    cmd[7] = fcc[1];
                    break;
                default:
                    break;
            }

            LogUtil.Logger.Debug(string.Format("{0} send {1}: {2}",slaveCode,currentCmdType,   ScaleHelper.HexBytesToString(cmd)));
            sp.Write(cmd, 0, cmd.Length);
            if (needAK)
            {
                SynReceiveData();
            }
            return true;
        }

       
        //同步阻塞读取    
        private void SynReceiveData()
        {
            try
            {
                byte firstByte = Convert.ToByte(this.sp.ReadByte());

                int bytesRead = this.sp.BytesToRead;
                byte[] bytesData = new byte[bytesRead + 1];
                bytesData[0] = firstByte;
                for (int i = 1; i <= bytesRead; i++)
                {
                    bytesData[i] = Convert.ToByte(this.sp.ReadByte());
                }
                string ss = ScaleHelper.HexBytesToString(bytesData);

                this.resendCount = 0;
                LogUtil.Logger.Debug("接收到数据："+ScaleHelper.HexBytesToString(bytesData));

                // 解析返回
                if (this.currentCmdType == CmdType.START_TEST || this.currentCmdType==CmdType.STOP_TEST)
                {
                    if (bytesData.Length == 8 || bytesData.Length == 9)
                    {
                        byte[] bcode = bytesData.Take(2).ToArray();
                        byte ack_nak = bytesData[4];

                        IESlave slave = FindSalveByBCode(bcode);
                        if (slave != null)
                        {
                            if (ack_nak == 0xB0)
                            {
                                slave.Status = SlaveStatus.OK_TO_TEST;
                            }
                            else if (ack_nak == 0xB1)
                            {
                                byte error = bytesData[6];
                                slave.Status = GetNakSlaveStatus(error);
                            }
                        }

                    }
                }else if (this.currentCmdType == CmdType.POLL_DATA)
                {
                    if (bytesData.Length >= 8)
                    {
                        byte[] bcode = bytesData.Take(2).ToArray();

                        IESlave slave = FindSalveByBCode(bcode);

                        byte ack_nak = bytesData[4];
                        if (ack_nak == 0xD0)
                        {
                            // 存在数据返回
                            int dataCount = ScaleHelper.HexByteToDecimal(bytesData[6]);
                            if (dataCount > 0)
                            {
                                List<int> times = new List<int>();
                                /// 每三字节为一个数据, 从第8个字节开始
                                for (int i = 0; i < dataCount; i++)
                                {
                                    byte[] data = bytesData.Skip(7 + 3 * i).Take(3).ToArray();
                                    times.Add(ScaleHelper.HexBytesToDecimal(data));
                                }
                                if (times.Contains(0))
                                {
                                    slave.Status = SlaveStatus.ON_CLOCKING;
                                }
                                else {
                                    slave.Status = SlaveStatus.OUT_CLOCKING;
                                }
                                List<IEData> datas = new List<IEData>();
                                foreach (int t in times)
                                {
                                    LogUtil.Logger.Info(slave.Code + "数据：" + t);
                                    datas.Add(new IEData() { Time = t });
                                }
                                slave.AddDatasToList(datas);
                                /// 给从机反馈，主机收到了数据
                                SendCmd(CmdType.ACK, slave.Code, false, false);
                            }
                            else {
                                slave.Status = SlaveStatus.OUT_CLOCKING;
                            }
                        }else if (ack_nak == 0xB1)
                        {
                            byte error = bytesData[6];
                            slave.Status = GetNakSlaveStatus(error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
               // ReSendCmd();
            }

        }

        /// <summary>
        /// 重新发送
        /// </summary>
        private void ReSendCmd()
        {
            // 重复1次
            if (this.resendCount <= 0)
            {
                this.resendCount += 1;
                LogUtil.Logger.Info("重新发送");
                SendCmd(this.currentCmdType, this.Slaves[this.currentSlaveIndex].Code, true);
            }
            else
            {
                resendCount = 0;
                throw new SlaveReponseTimeOutException("从机响应超时");
            }
        }
       
        /// <summary>
        /// 盘点串口是否打开
        /// </summary>
        /// <returns></returns>
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
                    this.sp.ReadTimeout = this.timeout;
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
        /// 生成SN
        /// </summary>
        private void GenerateSN()
        {
            if (currentSN == 255)
            {
                currentSN = 0x00;
            }
            currentSN += 1;
        }

        /// <summary>
        /// 根据bCode找到从机
        /// </summary>
        /// <param name="bcode"></param>
        /// <returns></returns>
        private IESlave FindSalveByBCode(byte[] bcode)
        {
            return this.Slaves.SingleOrDefault(s =>s.Code.Equals(ScaleHelper.HexBytesToString(bcode,false)));
        }

        /// <summary>
        /// 判断主机是否有从机
        /// </summary>
        /// <returns></returns>
        private bool CheckSlaves()
        {
            if(this.Slaves==null || this.Slaves.Count == 0)
            {
                throw new Exception("从机为空或数量为0");
            }
            return false;
        }

        /// <summary>
        /// 定时扫描数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PollDataTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            // 先停止timer
            pollDataTimer.Stop();
            // 获取数据
            if (started)
            {
                DoPollData();
            }
        }

        /// <summary>
        /// 获取从机数据
        /// </summary>
        private void DoPollData()
        {
            this.CheckSlaves();
            for (int i = 0; i < this.Slaves.Count; i++)
            {
                if (started)
                {
                    currentSlaveIndex = i;
                    try
                    {
                        currentCmdType = CmdType.POLL_DATA;
                        bool r = SendCmd(currentCmdType, this.Slaves[currentSlaveIndex].Code);

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
            }
            currentSlaveIndex = 0;
            // 开始timer
            pollDataTimer.Start();
        }
       
        /// <summary>
        /// 获取NAK的错误码的从机状态
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        private SlaveStatus GetNakSlaveStatus(byte error)
        {
            switch (error)
            {
                case 0x00:
                    return SlaveStatus.MSG_PARSE_ERROR;
                case 0x01:
                    return SlaveStatus.ID_NOT_MATCH;
                case 0x02:
                    return SlaveStatus.OUT_CLOCKING;
                default:
                    return SlaveStatus.NOK_TO_TEST;
            }
        }


    }
}
