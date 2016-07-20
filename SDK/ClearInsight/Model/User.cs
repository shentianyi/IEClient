using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;

namespace ClearInsight.Model
{
    class Part
    {
        public string email { get; set; }

        public string password { get; set; }
    }

    public class User
    {
        public string Email { get; set; }

        public string Password { get; set; }

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

            o["email"] = this.Email;
            o["password"] = this.Password;

            //Console.WriteLine("data-{0}", o);
            //Console.Read();
            return o;
        }

        //public string toJsonObject()
        //{
        //    JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
        //    Part part = new Part();
        //    part.email = this.Email;
        //    part.password = this.Password;
        //    string data = jsonSerializer.Serialize(part);

        //    Console.WriteLine("data-{0}", data);
        //    Console.Read();

        //    return data;
        //}
    }
}
