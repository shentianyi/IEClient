��ʼʹ��

.Net Framework 3.5 or ���߸���

��������:www.cz-tek.com:8000
���Ի���:www.cz-tek.com:8082

1.��� ClearInsight.dll , RestSharp.dll �� Newtonsoft.Json.dll �������Ŀ��

2.���ʹ��

��ʼ��API
ClearInsightAPI api = new ClearInsightAPI("https://www.cz-tek.com:8000","enter your access token here");

�����ṩ���ַ�ʽ���ϴ�KPI

//ͬ��
// ����Դ�����KpiEntry����KpiEntry���飬���batch����Ϊtrue����ֻ�е���KpiEntry������Ϊ������
//����ȫ����֤ͨ��ʱ���������Ż����Kpi
a.  CIResponse response = api.ImportKpiEntries();

//�첽
// ����Դ�����KpiEntry����KpiEntry���飬���batch����Ϊtrue����ֻ�е���KpiEntry������Ϊ������
//����ȫ����֤ͨ��ʱ���������Ż����Kpi
b.  api.ImportKpiEntriesAsync();

3.һ������������

�첽
Console.WriteLine("Test Bulk KpiEntries Async");
//��ʼ��KpiEntry
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
	property.Name = "�ͻ�����";
	property.Valye = "��Ҫ�ͻ�";
	entry.Attributes.Add(property);
        entries.Add(entry);
}
//����api
api.ImportKpiEntriesAsync(entries.ToArray(), res => { Console.WriteLine(res.ToString()); });

ͬ��
Console.WriteLine("Test Bulk KpiEntries");
//��ʼ��KpiEntry
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
	property.Name = "�ͻ�����";
	property.Valye = "��Ҫ�ͻ�";
	entry.Attributes.Add(property);
        entries.Add(entry);
}
//����api
CIResponse response = api.ImportKpiEntries(entries.ToArray());
//
Console.WriteLine(response.ToString());

3. ��λ��access token�����Ķ�pdf "doc/ApplyOAuth2.0AccessTokenV1.pdf"