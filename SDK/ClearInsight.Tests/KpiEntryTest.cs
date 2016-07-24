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
            api = new ClearInsightAPI("http://192.168.10.150:3000", "6ac975df20fd0e42c43f9ff0e3cfe7d36004e16471d0eb3951460285d74260bd");
        }

        public void TestUserLogin()
        {
            Console.WriteLine("Test User Login");
            //init user
            User user = new User();
            user.email = "admin@ci.com";
            user.password = "123456@";

            //call api
            User user2 = api.UserLogin(user);
            //
            Console.WriteLine(user2.ToString());
        }

        public void TestUserLogout()
        {
            Console.WriteLine("Test User Logout");
            //init user
            User user = new User();
            user.email = "admin@ci.com";
            user.password = "123456@";

            //call api
            CIResponse response = api.UserLogout(user);
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
            entry.value = "2222";
            entry.entry = new System.DateTime();

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
            List<Project> projects  = api.GetMProjects(ProjectStatus.ON_GOING);

            //
            Console.WriteLine(projects);
        }

        public void TestGetWorkUnitNodes()
        {
            Console.WriteLine("Test Get Work Unit Nodes");
            //init params
            //call api
            List<Node> nodes = api.GetWorkUnitNodes(5);

            //output
            Console.WriteLine(nodes);
        }

    }
}
