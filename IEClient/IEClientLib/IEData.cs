using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IEClientLib
{
    public class IEData
    {
        public IEData()
        {
            this.Stored = false;
            this.Valid = true;
            this.Time = 0;
        }

        /// <summary>
        /// 是否被保存为数据载体，默认是false
        /// </summary>
        public bool Stored { get; set; }
        
        /// <summary>
        /// 是否是合法的数据，默认是true
        /// </summary>
        public bool Valid { get; set; }

        /// <summary>
        /// 时间长度
        /// </summary>
        public int Time { get; set; }


    }
}
