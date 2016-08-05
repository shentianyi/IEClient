using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClearInsight;
using ClearInsight.Model;
using ClearInsight.Helper;

namespace ClearInsight.Tests
{
    public class KpiEntryTest
    {
        private ClearInsightAPI api;
        public KpiEntryTest()
        {
            api = new ClearInsightAPI("http://192.168.1.27:3000", "c13d9ba34f4cf028f8a1416869799c7bf42cf474f5c51b0bf3d9912c638f39e4");
        }

        public void TestUserLogin()
        {
            Console.WriteLine("Test User Login");

            //call api
            var user2 = api.UserLogin("admin@ci.com", "123456@");
            //
            Console.WriteLine(user2.toJson());
        }

        public void TestUserLogout()
        {
            Console.WriteLine("Test User Logout");

            //call api
            CIResponse response = api.UserLogout("admin@ci.com", "123456@");
            Console.WriteLine(response.ToString());
        }

        public void TestUploadKpiEntry()
        {
            Console.WriteLine("Test Upload Kpi Entry");
            //init entry
            KpiEntry entry = new KpiEntry();
            entry.kpi_id = 1;
            entry.project_item_id = 1;
            entry.tenant_id = 1;
            entry.node_id = 1;
            entry.node_code = "1";
            entry.node_uuid = "1";
            entry.value = 30;
            entry.entry_at = new System.DateTime();

            //call api
            KpiEntry ke = api.UploadKpiEntry(entry);
            //
            Console.WriteLine(ke.ToString());
        }

        public void TestGetProjects()
        {
            Console.WriteLine("Test Get Projects");
            //init params
            //call api
            List<Project> projects = api.GetProjects(ProjectStatus.FINISHED);

            //
            Console.WriteLine(projects);
        }

        public void TestGetWorkUnitNodes()
        {
            Console.WriteLine("Test Get Work Unit Nodes");
            //init params
            //call api
            List<Node> nodes = api.GetWorkUnitNodes(1);

            //output
            Console.WriteLine(nodes);
        }

    }
}
