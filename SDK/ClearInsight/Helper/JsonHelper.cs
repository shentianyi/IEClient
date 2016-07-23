using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Web.Script.Serialization;

namespace ClearInsight.Helper
{
    public class JsonHelper
    {
        /// <summary>
        /// json反序列化（非二进制方式）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static T JsonDeserialize<T>(string jsonString)
        {
            Console.WriteLine(jsonString);
            JavaScriptSerializer jsonSerialize = new JavaScriptSerializer();
            return (T)jsonSerialize.Deserialize<T>(jsonString);
        }
    }
}
