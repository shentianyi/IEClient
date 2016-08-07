using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IEClientLib.Enums;
using IEClientLib.Helper;
using System.ComponentModel;
using System.Timers;

namespace IEClientLib
{
    /// <summary>
    /// 从机
    /// </summary>
    public class IESlave<T> : INotifyPropertyChanged
    {
        private static List<SlaveStatus> showStautsClockStatuses = new List<SlaveStatus>() { SlaveStatus.ON_CLOCKING, SlaveStatus.OUT_CLOCKING };
        public IESlave()
        {
            this.selected = false;
            this.Status = SlaveStatus.OFF;
            this.statusTimer = new Timer();
            this.statusTimer.Interval = 1000;
            this.statusTimer.Elapsed += StatusTimer_Elapsed;
            this.statusTimer.Start();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        /// event
        // 状态改变事件
        public delegate void StatusChangedEventHandler(IESlave<T> slave);
        public event StatusChangedEventHandler StatusChanged;
        // 计时事件
        public delegate void TimeTickedEventHandler(IESlave<T> slave, IEData<T> data);
        public event TimeTickedEventHandler TimeTicked;
        
        private string code;
        private SlaveStatus status;
        private List<IEData<T>> dataList = new List<IEData<T>>();
        private bool? selected;

        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        ///// <summary>
        ///// 外部Id
        ///// </summary>
        //public int ExtId { get; set; }

        /// <summary>
        /// 外部实例
        /// </summary>
        public T ExtItem { get; set; }

        private string barCode;
        /// <summary>
        /// 条码编号
        /// </summary>
        public string BarCode
        {
            get { return barCode; }
            set
            {
                barCode = value;
                string id = barCode.Substring(2, barCode.Length - 2);
                string hex = ScaleHelper.DecimalToHexString(int.Parse(id), true, 2);
                this.Code = "55" + hex;
                OnPropertyChanged(new PropertyChangedEventArgs("BarCode"));
            }
        }
        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return code; }
            set
            {
                code = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Code"));
                if (!string.IsNullOrWhiteSpace(code))
                {
                    this.BCode = ScaleHelper.HexStringToHexByte(code);
                }
            }
        }
        ///// <summary>
        ///// 外部编码
        ///// </summary>
        //public string ExtCode { get; set; }

        public string Name { get; set; }
        
        /// <summary>
        /// 编码为字符数组,在赋值Code时同时自动转换为BCode
        /// </summary>
        public byte[] BCode { get; set; }

        /// <summary>
        /// 当前状态
        /// </summary>
        public SlaveStatus Status
        {
            get
            {
                return status;
            }
            set
            {
                if (status != value)
                {

                    status = value;
                     
                    this.GtOnClockingMax = false;
                    this.GtOutClockingMax = false;

                    this.StatusClockTick = 0;
                    OnPropertyChanged(new PropertyChangedEventArgs("Status"));

                    OnPropertyChanged(new PropertyChangedEventArgs("StatusDispaly"));
                    OnPropertyChanged(new PropertyChangedEventArgs("ShowStatusClock"));

                    if (this.StatusChanged != null)
                    {
                        this.StatusChanged(this);
                    }
                }
            }
        }

        /// <summary>
        /// 当前的命令类型
        /// </summary>
        public CmdType CurrentCmdType { get; set; }

        public string StatusDispaly {
            get { return EnumHelper.GetDescription(this.status); }
        }

        /// <summary>
        /// 需要变成的状态
        /// </summary>
        public SlaveStatus ToStauts {
            get; set;
        }

        private int bettery = 100;
        public int Battery
        {
            get { return bettery; }
            set {
                
                bettery = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Battery"));
                OnPropertyChanged(new PropertyChangedEventArgs("BatteryDisplay"));
                OnPropertyChanged(new PropertyChangedEventArgs("BatteryStage"));
                OnPropertyChanged(new PropertyChangedEventArgs("BatteryWidth"));
            }
        }
        public int BatteryWidth
        {
            get { return bettery/2; }
             
        }
        public string BatteryDisplay
        {
            get { return string.Format("{0}%",this.bettery); }
        }
        public int BatteryStage
        {
            get
            {
                if (this.bettery > 15)
                {
                    return 1;
                }
                else
                    return 0;
            }
        }

        //从机运行状态颜色提示
        public bool? Selected
        {
            get { return selected; }
            set
            {
                selected = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Selected"));
            }
        }

      
        /// <summary>
        /// IEData 列表
        /// </summary>
        public List<IEData<T>> DataList { get { return dataList; } set { this.dataList = value; } }
         
        public float? MaxFilter { get; set; }
        public float? MinFilter { get; set; }

        private Timer statusTimer;

        private int outClockingMax;
        private int onClockingMax;
        private bool gtOutClockingMax;
        private bool gtOnClockingMax;

        private int statusClockTick=0;

        public int OutClockingMax
        {
            get { return this.outClockingMax; }
            set
            {
                this.outClockingMax = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OutClockingMax"));
            }
        }

        public int OnClockingMax
        {
            get { return this.onClockingMax; }
            set
            {
                this.onClockingMax = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OnClockingMax"));
            }
        }

        public int StatusClockTick
        {
            get
            {
                return this.statusClockTick;
            }

            set
            {
                this.statusClockTick = value;
                if (this.Status == SlaveStatus.OUT_CLOCKING)
                {
                    this.GtOutClockingMax = this.StatusClockTick > this.OutClockingMax;
                }
                else if (this.Status == SlaveStatus.ON_CLOCKING)
                {
                    this.GtOnClockingMax = this.StatusClockTick > this.OnClockingMax;
                }
                else
                {
                    this.GtOutClockingMax = false;
                    this.GtOnClockingMax = false;
                }
                OnPropertyChanged(new PropertyChangedEventArgs("StatusClockTick"));
            }
        }

        public bool GtOutClockingMax
        {
            get
            {
                return gtOutClockingMax;
            }

            set
            {
                gtOutClockingMax = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GtOutClockingMax"));
                OnPropertyChanged(new PropertyChangedEventArgs("GtClockingMax"));
            }
        }
        public bool GtOnClockingMax
        {
            get
            {
                return gtOnClockingMax;
            }

            set
            {
                gtOnClockingMax = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GtOnClockingMax"));
                OnPropertyChanged(new PropertyChangedEventArgs("GtClockingMax"));

            }
        }

        public bool GtClockingMax
        {
            get { return this.GtOutClockingMax || this.GtOnClockingMax; }
        }

        public bool ShowStatusClock
        {
            get { return showStautsClockStatuses.Contains(this.Status); }
        }

    

        private void StatusTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.StatusClockTick += ((int)this.statusTimer.Interval / 1000);
        }

        public void AddDatasToList(List<IEData<T>> datas)
        {
            for (int i = 0; i < datas.Count; i++)
            {
                if (datas[i].Time != 0)
                {
                    datas[i].Nr = this.dataList.Count + i + 1;
                    this.dataList.Add(datas[i]);
                    if (this.TimeTicked != null)
                    {
                        this.TimeTicked(this, datas[i]);
                    }
                }
            }
        }
    }
}