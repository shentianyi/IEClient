using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;

namespace ClearInsight.Model
{
    public class User
    {
        public string email { get; set; }

        public string password { get; set; }

        public int id { get; set; }

        public string token { get; set; }

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

            o["email"] = this.email;
            o["password"] = this.password;

            //Console.WriteLine("data-{0}", o);
            //Console.Read();
            return o;
        }
    }
}
