using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClearInsight.Model
{ 
    public class Msg<T>
    {
        public bool result { get; set; }
        public T data { get; set; }
    }
}
