using System;
using System.Collections;
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
    public class IEHost<T>
    {
        /// <summary>
        /// 串口参数
        /// </summary>
        private SerialPort sp { get; set; }
        private string Com { get; set; }
        private int BaudRate;
        private Parity Parity;

        private bool comIsClosing = false;
        private bool comIsListening = false;

        /// <summary>
        /// 接受超时及重发
        /// </summary>
        private int timeout = 300;
        private int resendCount = 0;

        /// <summary>
        /// 从机及当前的变量
        /// </summary>
        public List<IESlave<T>> Slaves { get; set; }//从机列表
     //  private int currentSlaveIndex = 0;//从机序号
     //   private CmdType currentCmdType; //命令类型
        private byte currentSN = 0x00; //数据帧编号0-255
        private byte currentTaskId = 0x01; //任务ID
        private bool started = false;

        /// <summary>
        /// 命令队列
        /// </summary>
        private Queue cmdSyncQueue;
        private Queue cmdQueue;
        Thread cmdHandlerThread;
        ManualResetEvent cmdEvent;

        static object nextCmdLocker = new object() ;
        bool canNextCmd = true;

        private bool res = false;
        private byte firstByte=0x00;
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

            /// 初始化命令队列
            this.cmdSyncQueue = new Queue();
            this.cmdHandlerThread = new Thread(CmdHandler);
            this.cmdQueue = Queue.Synchronized(cmdSyncQueue);
            this.cmdEvent = new ManualResetEvent(false);
            this.cmdHandlerThread.Start();
            this.canNextCmd = true;
        }

        /// <summary>
        /// 发送命令
        /// </summary>
        /// <param name="cmdtype">命令类型</param>
        /// <param name="slaveCodes">从机编号列表</param>
        public void DoSendCmd(CmdType cmdtype, List<string> slaveCodes)
        {
            foreach (var code in slaveCodes)
            {
                if (!string.IsNullOrWhiteSpace(code))
                {
                    Dictionary<string, CmdType> dic = new Dictionary<string, CmdType>();
                    dic.Add(code, cmdtype);
                    cmdQueue.Enqueue(dic);
                    cmdEvent.Set();
                }
            }
        }


        private void CmdHandler() {
            while (true)
            {
                
                while(cmdQueue.Count>0 && canNextCmd)
                {
                    Dictionary<string, CmdType> dic=  cmdQueue.Peek() as Dictionary<string, CmdType>;
                    CmdType cmdType = dic.Values.First();
                    string code = dic.Keys.First();
                    IESlave<T> slave = this.FindSlaveByCode(code);
                    if (slave != null)
                    {
                        slave.CurrentCmdType = cmdType;
                        SendCmd(slave);
                    }
                }
                cmdEvent.WaitOne();
                cmdEvent.Reset();
            }
        }
             

        ///// <summary>
        ///// 开始测试
        ///// </summary>
        //public void StartTest(List<string> codes)
        //{
        //    this.started = true;
        //    //  StartOrStopTest(CmdType.START_TEST);
        //    DoSendCmd(CmdType.START_TEST, codes);
        //}

        /// <summary>
        /// 获取数据
        /// </summary>
        public void PollData()
        {
            DoPollData();
        }

        ///// <summary>
        ///// 停止测试
        ///// </summary>
        //public void StopTest(List<string> codes)
        //{
        //    started = false;
        //    this.pollDataTimer.Stop();
        //    this.pollDataTimer.Enabled = false;
        //    Thread.Sleep(3000);
        //    //StartOrStopTest(CmdType.STOP_TEST);
        //    DoSendCmd(CmdType.STOP_TEST, codes);
        //    Thread.Sleep(3000);
        //   // Close();
        //}
        
        /// <summary>
        /// 停止测试
        /// </summary>
        public void ShutDown()
        {
            this.cmdHandlerThread.Abort();
        }

        /// <summary>
        /// 开始或停止测试
        /// </summary>
        /// <param name="cmdType"></param>
        //private void StartOrStopTest(CmdType cmdType)
        //{
        //    this.CheckSlaves();
        //    for (int i = 0; i < this.Slaves.Count; i++)
        //    {
        //        currentSlaveIndex = i;
        //        try
        //        {
        //            currentCmdType = cmdType;
        //            bool r = SendCmd(cmdType, this.Slaves[currentSlaveIndex].Code);
        //        }
        //        catch (SlaveReponseTimeOutException ex)
        //        {
        //            LogUtil.Logger.Error(ex.Message + ":" + this.Slaves[currentSlaveIndex].Code);
        //        }
        //        catch (OpenComException ex)
        //        {
        //            throw ex;
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //            // LogUtil.Logger.Error(ex.Message + ":" + this.Slaves[currentSlaveIndex].Code);
        //        }
        //    }
        //    currentSlaveIndex = 0;
        //}


        /// <summary>
        /// 发送命令
        /// </summary>
        /// <param name="cmdType">命令类型</param>
        /// <param name="slaveCode">从机编码</param>
        /// <param name="reSend">是否是重发</param>
        /// <param name="needAK">是否需要从机确认</param>
        /// <returns></returns>
        private bool SendCmd( IESlave<T> slave, bool reSend = false, bool needAK = true)
        {
            try
            {
                if (!reSend)
                {
                    resendCount = 0;
                }
                res = false;
                Thread.Sleep(100);
             

                LogUtil.Logger.Debug("start send.................");
                if (!IsOpen())
                {
                    OpenCom();
                }
              //  currentCmdType = slave.CurrentCmdType;

                byte[] codeBytes = ScaleHelper.HexStringToHexByte(slave.Code);
                if (!reSend)
                {
                    GenerateSN();
                }
                byte[] cmd = new byte[9];
                byte[] fcc = new byte[2];

                switch (slave.CurrentCmdType)
                {
                    case CmdType.START_TEST:
                    case CmdType.STOP_TEST:
                        cmd[0] = codeBytes[0];
                        cmd[1] = codeBytes[1];
                        cmd[2] = 0xAA; // STX
                        cmd[3] = 0x03; // LEN
                        cmd[4] = (byte)(slave.CurrentCmdType == CmdType.START_TEST ? 0xC0 : 0xC1); // CMD
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

                LogUtil.Logger.Debug(string.Format("{0} send {1}: {2}", slave.Code, slave.CurrentCmdType, ScaleHelper.HexBytesToString(cmd)));
                sp.Write(cmd, 0, cmd.Length);
                firstByte = Convert.ToByte(this.sp.ReadByte());

                if (needAK)
                {
                    while (!res)
                    {

                    }
                    this.DeQueueCmd();
                    // SynReceiveData();
                    //ReceiveDataThread(this.sp);
                }

                return true;
            }
            catch (TimeoutException ex)
            {
                if (started || slave.CurrentCmdType == CmdType.STOP_TEST || slave.CurrentCmdType==CmdType.START_TEST)
                {
                    ReSendCmd(slave);
                }
            }
            return false;

        }
         


        /// <summary>
        /// 重新发送
        /// </summary>
        private void ReSendCmd(IESlave<T> slave)
        {
            try {
                int resentMax = 1;
                if (slave.CurrentCmdType == CmdType.START_TEST || slave.CurrentCmdType == CmdType.STOP_TEST) {
                    resentMax = 3;
                }
                else
                {
                    resentMax = 1;
                }
                // 重复1次
                if (this.resendCount < resentMax)
                {
                    this.resendCount += 1;
                    LogUtil.Logger.Info("重新发送");
                    SendCmd(slave, true);
                }
                else
                {
                    resendCount = 0;
                    this.DeQueueCmd();
                    throw new SlaveReponseTimeOutException(new Exception());
                }
            }catch(Exception ex)
            {
                LogUtil.Logger.Error(ex.Message + ":" + slave.Code);
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
                    this.sp.DataReceived += Sp_DataReceived;
                }
                this.sp.Open();
                return true;
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Error(ex.Message);
                throw new OpenComException(ex);
            }
        }

        private void Sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (comIsClosing) return;
            try
            {
                comIsListening = true;
                if (sp.IsOpen)
                {
                    Thread.Sleep(100);

                    if (comIsClosing) return;
                    byte[] bytesData = new byte[sp.BytesToRead];
                    sp.Read(bytesData, 0, bytesData.Length);
                    if (firstByte != 0x00)
                    {
                        byte[] tmp = new byte[1 + bytesData.Length];
                        tmp[0] = firstByte;
                        for (int i = 0; i < bytesData.Length; i++)
                        {
                            tmp[i + 1] = bytesData[i];
                        }
                        bytesData = tmp;
                    }
                    LogUtil.Logger.Error(ScaleHelper.HexBytesToString(bytesData));
                    Parse(bytesData);
                    res = true;
                }
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Info(ex.Message);
            }
            finally {
                comIsListening = false;
            }
        }

        /// <summary>
        /// 关闭串口
        /// </summary>
        /// <returns></returns>
        public bool Close(bool reclose=false)
        {
            if (this.sp != null)
            {
                try
                {
                    if (this.sp.IsOpen)
                    {
                        comIsClosing = true;
                        while(comIsListening) { }
                        this.sp.Close();
                        LogUtil.Logger.Info("Close Success");
                        comIsClosing = false;
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    if (!reclose)
                    {
                        Close(true);
                    }
                    LogUtil.Logger.Error("Close Error");
                    LogUtil.Logger.Error(ex.Message);
                    if (reclose)
                    {
                        throw ex;
                    }
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
        private IESlave<T> FindSalveByBCode(byte[] bcode)
        {
            return this.Slaves.SingleOrDefault(s => s.Code.Equals(ScaleHelper.HexBytesToString(bcode, false)));
        }

        /// <summary>
        /// 根据code找到从机
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private IESlave<T> FindSlaveByCode(string code)
        {
            return this.Slaves.SingleOrDefault(s => s.Code.Equals(code));
        }

        /// <summary>
        /// 判断主机是否有从机
        /// </summary>
        /// <returns></returns>
        private bool CheckSlaves()
        {
            if (this.Slaves == null || this.Slaves.Count == 0)
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
           // if (started)
           // {
                DoPollData();
           // }
        }

        /// <summary>
        /// 获取从机数据
        /// </summary>
        private void DoPollData()
        {
            if (comIsClosing)
            {
                return;
            }
            //this.CheckSlaves();
            if (this.cmdQueue.Count == 0)
            {
                for (int i = 0; i < this.Slaves.Count; i++)
                {
                    //if (started)
                    //{
                        List<string> codes = new List<string>();
                        //try
                        //{
                            if (this.Slaves[i].Status != SlaveStatus.OFF)
                            {
                                //currentCmdType = CmdType.POLL_DATA;
                                //bool r = SendCmd(this.Slaves[i]);
                                codes.Add(this.Slaves[i].Code);
                            }
                        //}
                        //catch (SlaveReponseTimeOutException ex)
                        //{
                        //    LogUtil.Logger.Error(ex.Message + ":" + this.Slaves[i].Code);
                        //}
                        //catch (Exception ex)
                        //{
                        //    LogUtil.Logger.Error(ex.Message + ":" + this.Slaves[i].Code);
                        //}

                        this.DoSendCmd(CmdType.POLL_DATA, codes);
                  //  }

                }

            }
            // 开始timer
            pollDataTimer.Start();
        }

        /// <summary>
        /// 获取NAK的错误码的从机状态
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        private SlaveStatus GetNakSlaveStatus(byte error, IESlave<T> slave)
        {
            switch (error)
            {
                case 0x00:
                    return SlaveStatus.MSG_PARSE_ERROR;
                case 0x01:
                    return SlaveStatus.ID_NOT_MATCH;
                case 0x02:
                    return slave.CurrentCmdType==CmdType.STOP_TEST ? SlaveStatus.OFF : SlaveStatus.OUT_CLOCKING;
                default:
                    return SlaveStatus.NOK_TO_TEST;
            }
        }

        /// <summary>
        /// 解析数据
        /// </summary>
        /// <param name="bytesData"></param>
        private void Parse(byte[] bytesData)
        {
            if (bytesData.Length > 7)
            {
                byte[] bcode = bytesData.Take(2).ToArray();
                byte ack_nak = bytesData[4];

              

                IESlave<T> slave = FindSalveByBCode(bcode);
                if (slave != null)
                {
                    // parse bettery
                    byte bettery = bytesData.Skip(bytesData.Length - 3).Take(1).First();
                    LogUtil.Logger.Info(slave.Code + "电量：" + ScaleHelper.HexByteToDecimal(bettery));
                    slave.Battery = ScaleHelper.HexByteToDecimal(bettery);


                    // 解析返回
                    if (slave.CurrentCmdType == CmdType.START_TEST || slave.CurrentCmdType == CmdType.STOP_TEST)
                    {
                        if (bytesData.Length == 9 || bytesData.Length == 10)
                        {

                            if (ack_nak == 0xB0)
                            {
                                slave.Status = slave.CurrentCmdType == CmdType.START_TEST ? SlaveStatus.OK_TO_TEST : SlaveStatus.OFF;
                            }
                            else if (ack_nak == 0xB1)
                            {
                                byte error = bytesData[6];
                                slave.Status = GetNakSlaveStatus(error,slave);
                            }
                            else if (ack_nak == 0xD0)
                            {
                                /// 给从机反馈，主机收到了数据 TODO ACK
                                // SendCmd(CmdType.ACK, slave.Code, false, false);

                                ParsePollData(bytesData, slave);
                            }



                        }
                    }
                    else if (slave.CurrentCmdType == CmdType.POLL_DATA)
                    {
                        if (bytesData.Length >= 8)
                        {
                            if (ack_nak == 0xD0)
                            {
                                ParsePollData(bytesData, slave);
                                /// 给从机反馈，主机收到了数据 TODO ACK
                                // SendCmd(CmdType.ACK, slave.Code, false, false);

                            }
                            else if (ack_nak == 0xB1)
                            {
                                byte error = bytesData[6];
                                slave.Status = GetNakSlaveStatus(error,slave);
                            }
                        }
                    }
                }
            }

        }

        /// <summary>
        /// 解析计时数据，每一组三个字节
        /// </summary>
        /// <param name="bytesData"></param>
        /// <param name="slave"></param>
        private void ParsePollData(byte[] bytesData,IESlave<T> slave)
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
                else
                {
                    slave.Status = SlaveStatus.OUT_CLOCKING;
                }
                List<IEData<T>> datas = new List<IEData<T>>();
                foreach (int t in times)
                {
                    LogUtil.Logger.Info(slave.Code + "数据：" + t);
                    datas.Add(new IEData<T>() { Time = t,Slave=slave});
                }
                slave.AddDatasToList(datas);
                /// 给从机反馈，主机收到了数据 TODO ACK
                // SendCmd(CmdType.ACK, slave.Code, false, false);
            }
            else
            {
                slave.Status = slave.CurrentCmdType == CmdType.STOP_TEST ? SlaveStatus.OFF : SlaveStatus.OUT_CLOCKING;
            }

        }

        /// <summary>
        /// 反转是否可以执行下一个命令的标记
        /// </summary>
        private void ReverseCanNext()
        {
            lock (nextCmdLocker)
            {
                this.canNextCmd = !this.canNextCmd;
            }
        }

        /// <summary>
        /// 当有响应或超时时，将第一个cmd出队列
        /// </summary>
        private void DeQueueCmd()
        {
            this.cmdQueue.Dequeue();
        }
    }
}
