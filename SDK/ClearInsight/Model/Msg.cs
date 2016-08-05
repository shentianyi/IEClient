using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace ClearInsight.Model
{ 
    public class Msg<T>
    {
        public bool result { get; set; }
        public string content { get; set; }
        public T data { get; set; }



        /// <summary>
        /// Function<c>toJson</c>
        /// </summary>
        /// <returns><c>string</c></returns>
        public string toJson()
        {
            return toJsonObject().ToString();
            //return toJsonObject();
        }

        /// <summary>
        /// Function<c>toJsonObject</c>
        /// return JObject
        /// </summary>
        /// <returns>JObject</returns>
        public JObject toJsonObject()
        {
            JObject o = new JObject();

            o["result"] = this.result;
            o["content"] = this.content;
            if (this.data != null) {
                o["data"] = this.data.ToString();
            }
            
            return o;
        }
    }
}
