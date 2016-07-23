using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ClearInsight.Model
{
    /// <summary>
    /// Class <c>KpiEntry</c>
    /// Model for Kpi Entry
    /// </summary>
    public class KpiEntry
    {
        /// <summary>
        /// Property <c>KpiID</c>
        /// id of kpi
        /// <remarks>should not be empty</remarks>
        /// </summary>
        public int kpi_id { get; set; }

        /// <summary>
        /// Property <c>Project Item ID</c>
        /// id of project item
        /// <remarks>should not be empty</remarks>
        /// </summary>
        public int project_item_id { get; set; }

        /// <summary>
        /// Property <c>Tenant ID</c>
        /// id of tenant
        /// <remarks>should not be empty</remarks>
        /// </summary>
        public int tenant_id { get; set; }

        /// <summary>
        /// Property <c>Node ID</c>
        /// id of node
        /// <remarks>should not be empty</remarks>
        /// </summary>
        public int node_id { get; set; }

        /// <summary>
        /// Property <c>Node Code</c>
        /// code of node
        /// <remarks>should not be empty</remarks>
        /// </summary>
        public string node_code { get; set; }

        /// <summary>
        /// Property <c>Node Uuid</c>
        /// uuid of node
        /// <remarks>should not be empty</remarks>
        /// </summary>
        public string node_uuid { get; set; }

        /// <summary>
        /// Property <c>Value</c>
        /// value of kpi entry
        /// <remarks>should not be empty</remarks>
        /// </summary>
        public string value { get; set; }

        /// <summary>
        /// Property <c>Entry</c>
        /// entry of kpi entry
        /// <remarks>should not be empty</remarks>
        /// </summary>
        public DateTime entry { get; set; }

        public string id { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }

        /// <summary>
        /// Function<c>toJson</c>
        /// </summary>
        /// <returns><c>string</c></returns>
        public string toJson()
        {
            return toJsonObject().ToString();
        }

        /// <summary>
        /// Function<c>toJsonObject</c>
        /// return JObject
        /// </summary>
        /// <returns>JObject</returns>
        public JObject toJsonObject()
        {
            JObject o = new JObject();

            o["kpi_id"] = this.kpi_id;
            o["project_item_id"] = this.project_item_id;
            o["tenant_id"] = this.tenant_id;
            o["node_id"] = this.node_id;
            o["node_code"] = this.node_code;
            o["node_uuid"] = this.node_uuid;
            o["value"] = this.value;
            o["entry"] = this.entry;

            return o;
        }
    }
}
