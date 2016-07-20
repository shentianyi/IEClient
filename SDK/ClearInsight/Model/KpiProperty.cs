using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ClearInsight.Model
{
    /// <summary>
    /// Class<c>KpiProperty</c>
    /// Model for Kpi Property
    /// </summary>
    public class KpiProperty
    {
        /// <summary>
        /// Property <c>Name</c>
        /// name of kpiproperty,
        /// <remarks></remarks>
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Property <c>Value</c>
        /// Value of kpiproperty,
        /// <remarks></remarks>
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Function<c>toJson</c>
        /// get json string
        /// </summary>
        /// <returns>string</returns>
        public string toJson()
        {
            JObject o = new JObject();
            o[this.Name] = this.Value;
            return o.ToString();
        }
    }
}
