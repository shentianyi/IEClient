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
        public string KpiID { get; set; }

        /// <summary>
        /// Property <c>Project Item ID</c>
        /// id of project item
        /// <remarks>should not be empty</remarks>
        /// </summary>
        public string ProjectItemID { get; set; }

        /// <summary>
        /// Property <c>Tenant ID</c>
        /// id of tenant
        /// <remarks>should not be empty</remarks>
        /// </summary>
        public string TenantID { get; set; }

        /// <summary>
        /// Property <c>Node ID</c>
        /// id of node
        /// <remarks>should not be empty</remarks>
        /// </summary>
        public string NodeID { get; set; }

        /// <summary>
        /// Property <c>Node Code</c>
        /// code of node
        /// <remarks>should not be empty</remarks>
        /// </summary>
        public string NodeCode { get; set; }

        /// <summary>
        /// Property <c>Node Uuid</c>
        /// uuid of node
        /// <remarks>should not be empty</remarks>
        /// </summary>
        public string NodeUuid { get; set; }

        /// <summary>
        /// Property <c>Value</c>
        /// value of kpi entry
        /// <remarks>should not be empty</remarks>
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Property <c>Entry</c>
        /// entry of kpi entry
        /// <remarks>should not be empty</remarks>
        /// </summary>
        public string Entry { get; set; }

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

            o["kpi_id"] = this.KpiID;
            o["project_item_id"] = this.ProjectItemID;
            o["tenant_id"] = this.TenantID;
            o["node_id"] = this.NodeID;
            o["node_code"] = this.NodeCode;
            o["node_uuid"] = this.NodeUuid;
            o["value"] = this.Value;
            o["entry"] = this.Entry;

            return o;
        }
    }
}
