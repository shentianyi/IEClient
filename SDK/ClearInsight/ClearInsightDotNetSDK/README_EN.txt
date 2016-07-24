Getting Started

.Net Framework 3.5 or higer is required.

Production Environment:www.cz-tek.com:8000
Test Environment:www.cz-tek.com:8082

1.Add ClearInsight.dll ,RestSharp.dll and Newtonsoft.Json.dll to your project

2.Usage

initialize a api
ClearInsightAPI api = new ClearInsightAPI("https://www.cz-tek.com:8000","enter your access token here");

we supply two api methods to upload your kpi

// you can pass KpiEntry or KpiEntry[],bool batch as parameter
// if batch = true ,all kpi entrie will rollback if error occurs
a.  CIResponse response = api.ImportKpiEntries();

// you can pass KpiEntry or KpiEntry[] ,and callback Action<CIResponse>, bool batch
// if batch = true ,all kpi entrie will rollback if error occurs
b.  api.ImportKpiEntriesAsync();

3.Full code, full examples please see https://github.com/shentianyi/SDK

Async
Console.WriteLine("Test Bulk KpiEntries Async");
//init kpientry
List<KpiEntry> entries = new List<KpiEntry>();
DateTime time = DateTime.Today;
for (int i = 0; i < 100; i++)
{
	KpiEntry entry = new KpiEntry();
	entry.KpiID = "1";
	entry.Value = "200";
        entry.Date = time.AddDays(i).ToString();
        entry.Email = "C-RBA_User@leoni.com";
	KpiProperty property = new KpiProperty();
	property.Name = "CustomerType";
	property.Valye = "VIP";
	entry.Attributes.Add(property);
        entries.Add(entry);
}
//cal api
api.ImportKpiEntriesAsync(entries.ToArray(), res => { Console.WriteLine(res.ToString()); });

Sync
Console.WriteLine("Test Bulk KpiEntries");
//init kpientry
List<KpiEntry> entries = new List<KpiEntry>();
DateTime time = DateTime.Today;
for (int i = 0; i < 100; i++)
{
 	KpiEntry entry = new KpiEntry();
        entry.KpiID = "1";
        entry.Value = "200";
        entry.Date = time.AddDays(i).ToString();
        entry.Email = "C-RBA_User@leoni.com";
	KpiProperty property = new KpiProperty();
	property.Name = "CustomerType";
	property.Valye = "VIP";
	entry.Attributes.Add(property);
        entries.Add(entry);
}
//cal api
CIResponse response = api.ImportKpiEntries(entries.ToArray());
//
Console.WriteLine(response.ToString());

3. How to get access token,please read pdf "doc/ApplyOAuth2.0AccessTokenV1.pdf"