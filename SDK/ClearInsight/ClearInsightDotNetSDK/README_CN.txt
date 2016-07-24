开始使用

.Net Framework 3.5 or 或者更高

开发环境:www.cz-tek.com:8000
测试环境:www.cz-tek.com:8082

1.添加 ClearInsight.dll , RestSharp.dll 和 Newtonsoft.Json.dll 到你的项目中

2.如何使用

初始化API
ClearInsightAPI api = new ClearInsightAPI("https://www.cz-tek.com:8000","enter your access token here");

我们提供两种方式来上传KPI

//同步
// 你可以传单个KpiEntry或者KpiEntry数组，如果batch参数为true，则只有当以KpiEntry数组作为参数，
//并且全部验证通过时，服务器才会插入Kpi
a.  CIResponse response = api.ImportKpiEntries();

//异步
// 你可以传单个KpiEntry或者KpiEntry数组，如果batch参数为true，则只有当以KpiEntry数组作为参数，
//并且全部验证通过时，服务器才会插入Kpi
b.  api.ImportKpiEntriesAsync();

3.一个完整的例子

异步
Console.WriteLine("Test Bulk KpiEntries Async");
//初始化KpiEntry
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
	property.Name = "客户类型";
	property.Valye = "重要客户";
	entry.Attributes.Add(property);
        entries.Add(entry);
}
//调用api
api.ImportKpiEntriesAsync(entries.ToArray(), res => { Console.WriteLine(res.ToString()); });

同步
Console.WriteLine("Test Bulk KpiEntries");
//初始化KpiEntry
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
	property.Name = "客户类型";
	property.Valye = "重要客户";
	entry.Attributes.Add(property);
        entries.Add(entry);
}
//调用api
CIResponse response = api.ImportKpiEntries(entries.ToArray());
//
Console.WriteLine(response.ToString());

3. 如何获得access token，请阅读pdf "doc/ApplyOAuth2.0AccessTokenV1.pdf"