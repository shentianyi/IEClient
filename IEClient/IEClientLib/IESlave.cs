﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IEClientLib.Enums;
using IEClientLib.Helper;
using System.ComponentModel;

namespace IEClientLib
{
    /// <summary>
    /// 从机
    /// </summary>
    public class IESlave<T> : INotifyPropertyChanged
    {
        public IESlave()
        {
            this.selected = false;
            this.Status = SlaveStatus.OFF;
            this.situation = "#FFFFFF";	
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
        private bool selected;
        private string situation;
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
        
        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return code; }
            set
            {
                code = value;
                OnPropertyChanged(new PropertyChangedEventArgs("code"));
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
        /// 状态
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

                    OnPropertyChanged(new PropertyChangedEventArgs("Status"));

                    if (this.StatusChanged != null)
                    {
                        this.StatusChanged(this);
                    }
                }
            }
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
            }
        }

        public string BatteryDisplay
        {
            get { return string.Format("{0}%",this.bettery); }
        }
        public int BatteryStage
        {
            get
            {
                if (this.bettery == 0)
                {
                    return 0;
                }
                else if (this.bettery > 0 && bettery <= 15)
                {
                    return 15;
                }
                return 100;
            }
        }

        //从机运行状态颜色提示
        public bool Selected
        {
            get { return selected; }
            set
            {
                selected = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Selected"));
            }
        }

        public string Situation
        {
            get { return situation; }
            set { situation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Situation"));
            }
        }
        /// <summary>
        /// IEData 列表
        /// </summary>
        public List<IEData<T>> DataList { get { return dataList; } set { this.dataList = value; } }
         
        public float? MaxFilter { get; set; }
        public float? MinFilter { get; set; }


        public void AddDatasToList(List<IEData<T>> datas)
        {
            foreach (IEData<T> data in datas)
            {
                if (data.Time != 0)
                {
                    this.dataList.Add(data);
                    if (this.TimeTicked != null)
                    {
                        this.TimeTicked(this, data);
                    }
                }
            }
        }
    }
}