using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IEClientLib.Enums;
using IEClientLib.Helper;

namespace IEClientLib
{
    /// <summary>
    /// 从机
    /// </summary>
    public class IESlave
    {
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
                this.BCode = ScaleHelper.HexStringToHexByte(code);
            }
        }
        /// <summary>
        /// 外部编码
        /// </summary>
        public int ExtCode { get; set; }

        
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

                    if (this.StatusChanged != null)
                    {
                        this.StatusChanged(this);
                    }
                }
            }
        }

        /// <summary>
        /// IEData 列表
        /// </summary>
        public List<IEData> DataList { get { return dataList; } set { this.dataList = value; } }

        public IESlave()
        {
            this.Status = SlaveStatus.OFF_LINE;
        }
        
        public void AddDatasToList(List<IEData> datas)
        {
            foreach (IEData data in datas)
            {
                this.dataList.Add(data);
                this.TimeTicked(this, data);
            }
        }
    }
}