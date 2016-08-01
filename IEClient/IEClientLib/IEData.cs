using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IEClientLib
{
    public class IEData<T>
    {
        public IEData()
        {
            this.Stored = false;
            this.Valid = true;
            this.Time = 0;
            this.PolledAt = DateTime.Now;
        }

        public IESlave<T> Slave { get; set; }

        /// <summary>
        /// 是否被保存为数据载体，默认是false
        /// </summary>
        public bool Stored { get; set; }

        /// <summary>
        /// 是否是合法的数据，默认是true
        /// </summary>
        public bool Valid { get; set; }

        /// <summary>
        /// 时间长度,100ms为单位
        /// </summary>
        public float Time { get; set; }

        /// <summary>
        /// 时间长度, 1s为单位
        /// </summary>
        public float ParsedTime { get {return this.Time / 10; } }

        /// <summary>
        /// 被抓取到的时间
        /// </summary>
        public DateTime PolledAt { get; set; }


    }
}
