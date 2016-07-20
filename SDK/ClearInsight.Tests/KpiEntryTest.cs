using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClearInsight;
using ClearInsight.Model;

namespace ClearInsight.Tests
{
    public class KpiEntryTest
    {
        private ClearInsightAPI api;
        public KpiEntryTest()
        {
            api = new ClearInsightAPI("http://192.168.1.27:3000", "a78cb4fa299c8cc8419bdede510e4d542bae4338570f0729ea1cf0a427e71914");
        }

        public void TestUserLogin()
        {
            Console.WriteLine("Test User Login");
            //init user
            User user = new User();
            user.Email = "admin@ci.com";
            user.Password = "111111";

            //call api
            CIResponse response = api.UserLogin(user);
            //
            Console.WriteLine(response.ToString());
        }

        public void TestUploadKpiEntry()
        {
            Console.WriteLine("Test Upload Kpi Entry");
            //init entry
            KpiEntry entry = new KpiEntry();
            entry.KpiID = "1";
            entry.ProjectItemID = "1";
            entry.TenantID = "1";
            entry.NodeID = "1";
            entry.NodeCode = "1";
            entry.NodeUuid = "1";
            entry.Value = "2222";
            entry.Entry = "2016-6-8";

            //call api
            CIResponse response = api.UploadKpiEntry(entry);
            //
            Console.WriteLine(response.ToString());
        }




    }
}
