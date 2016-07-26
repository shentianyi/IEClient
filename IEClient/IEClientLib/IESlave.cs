using System;
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
    public class IESlave : INotifyPropertyChanged
    {
        public IESlave()
        {
            this.selected = false;
            this.Status = SlaveStatus.OFF;
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
        public delegate void StatusChangedEventHandler(IESlave slave);
        public event StatusChangedEventHandler StatusChanged;
        // 计时事件
        public delegate void TimeTickedEventHandler(IESlave slave, IEData data);
        public event TimeTickedEventHandler TimeTicked;
        
        private string code;
        private SlaveStatus status;
        private List<IEData> dataList = new List<IEData>();
        private bool selected;
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 外部Id
        /// </summary>
        public int ExtId { get; set; }

        
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
        /// <summary>
        /// 外部编码
        /// </summary>
        public string ExtCode { get; set; }

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

        public bool Selected
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
        public List<IEData> DataList { get { return dataList; } set { this.dataList = value; } }
         
        
        public void AddDatasToList(List<IEData> datas)
        {
            foreach (IEData data in datas)
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