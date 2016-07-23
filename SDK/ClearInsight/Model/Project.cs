using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace ClearInsight.Model
{
    public enum ProjectStatus {
        ON_GOING=100,
        FINISHED=200
    }

    public class Project
    { 
        /// <summary>
        /// Property <c>Name</c>
        /// name of project
        /// <remarks>should not be empty</remarks>
        /// </summary>
        //public string Name { get; set; }
        public string name { get; set; }

        /// <summary>
        /// Property <c>Description</c>
        /// description of project
        /// <remarks>can be empty</remarks>
        /// </summary>
        //public string Description { get; set; }
        public string description { get; set; }

        /// <summary>
        /// Property <c>UserID</c>
        /// user id of project
        /// <remarks>should not be empty</remarks>
        /// </summary>
        //public string UserID { get; set; }
        public int user_id { get; set; }

        /// <summary>
        /// Property <c>Status</c>
        /// status of project
        /// <remarks>should not be empty</remarks>
        /// </summary>
        //public string Status { get; set; }
        public ProjectStatus status { get; set; }

        
        /// <summary>
        /// Property <c>TenantID</c>
        /// tenant id of project
        /// <remarks>should not be empty</remarks>
        /// </summary>
        //public string TenantID { get; set; }
        public int tenant_id { get; set; }

        /// <summary>
        /// Property <c>Remark</c>
        /// remark of project
        /// <remarks>can be empty</remarks>
        /// </summary>
        //public string Remark { get; set; }
        public string remark { get; set; }


        //
        public int id { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
         
    }
}
