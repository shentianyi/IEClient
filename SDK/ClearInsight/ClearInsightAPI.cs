using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClearInsight.Model;
using ClearInsight.Exception;
using ClearInsight.Validation;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Deserializers;
using RestSharp.Serializers;
using Newtonsoft.Json;
using ClearInsight.Helper;

namespace ClearInsight
{
    /// <summary>
    /// Class <c>ClierInsightAPI</c>Model for REST API
    /// </summary>
    public class ClearInsightAPI
    {
        /// <summary>
        /// Instalce Variable<c>_baseUrl</c> api base url
        /// </summary>
        string _baseUrl = "https://www.cz-tek.com:8000";
        /// <summary>
        /// Instance Variable<c>_accessToken</c> access Token
        /// </summary>
        string _accessToken = "";

        /// <summary>
        /// Constructor <c>ClearInsigheAPI</c>
        /// </summary>
        /// <param name="baseUrl">api base url,like "www.cz-tek.com".</param>
        public ClearInsightAPI(string baseUrl)
        {
            _baseUrl = baseUrl;
        }

        /// <summary>
        /// Constructor <c>ClearInsigheAPI</c>
        /// </summary>
        /// <param name="baseUrl">api base url,like "www.cz-tek.com".</param>
        /// <param name="accessToken">api access token.</param>
        public ClearInsightAPI(string baseUrl, string accessToken)
        {
            _baseUrl = baseUrl;
            _accessToken = accessToken;
        }

        /// <summary>
        /// Execute RESTSharp Request
        /// </summary>
        /// <typeparam name="T">Delegator</typeparam>
        /// <param name="request">RestSharp.RestRequest request</param>
        /// <returns>Delegator</returns>
        private T Execute<T>(RestRequest request) where T : new()
        {
            var client = new RestClient();
            client.BaseUrl = _baseUrl;
            client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(_accessToken, "Bearer");

            var response = client.Execute<T>(request);

            if(response.ErrorException != null)
            {
                throw new ClearInsightException(response.ErrorMessage);
            }
            return response.Data;
        }

        /// <summary>
        /// Execute RestSharp Request
        /// </summary>
        /// <param name="resquest">RestSharp.RestRequest request</param>
        /// <returns>CIResponse response</returns>
        private CIResponse Execute(RestRequest resquest)
        {
            var client = new RestClient();
            client.AddHandler("application.json", new JsonDeserializer());
            client.BaseUrl = _baseUrl;
            client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(_accessToken, "Bearer");
            IRestResponse response;
            response = client.Execute(resquest);
            
            return _processStatusCode(response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="callback"></param>
        private void ExecuteAsync(RestRequest request,Action<CIResponse> callback)
        {
            var client = new RestClient();
            client.AddHandler("application.json",new JsonDeserializer());
            client.BaseUrl = _baseUrl;
            client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(_accessToken, "Bearer");
            client.ExecuteAsync(request, response =>
            {
                callback(_processStatusCode(response));
            });
        }

        /// <summary>
        /// User Login
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns>CIResponse response</returns>
        public Msg<User> UserLogin(string email, string password)
        {
            var request = new RestRequest(Method.POST);

            request.Resource = "/api/v1/users/login";
            request.AddParameter("user", new User() { email = email, password = password }.toJson());

            //return Execute(request);
            return JsonHelper.JsonDeserialize<Msg<User>>(Execute(request).Content);
        }


        public CIResponse UserLogout(User user)
        {
            var request = new RestRequest(Method.POST);

            request.Resource = "/api/v1/users/logout";
            request.AddParameter("user", user.toJson());

            return Execute(request);
        }

        /// <summary>
        /// Upload Kpi Entry
        /// </summary>
        /// <param name="entry">ClearInsight.Model.KpiEntry</param>
        /// <returns>CIResponse response</returns>
        public KpiEntry UploadKpiEntry(KpiEntry entry)
        {
            var request = new RestRequest(Method.POST);

            request.Resource = "/api/v1/kpis/entries";
            request.AddParameter("entry", entry.toJson());

            //return Execute(request);
            return JsonHelper.JsonDeserialize<KpiEntry>(Execute(request).Content);
        }

        /// <summary>
        /// Get Projects
        /// </summary>
        /// <param name="status"></param>
        /// <returns>CIResponse response</returns>
        public List<Project> GetProjects(ProjectStatus Status = ProjectStatus.ON_GOING)
        {
            var request = new RestRequest(Method.GET);
            request.Resource = "/api/v1/projects";
            request.AddParameter("status", (int)Status);

            return JsonHelper.JsonDeserialize<List<Project>>(Execute(request).Content);
        }

        public List<Project> GetMProjects(ProjectStatus Status = ProjectStatus.ON_GOING)
        {
            var request = new RestRequest(Method.GET);
            request.Resource = "/api/v1/projects";
            request.AddParameter("status", (int)Status);

            return JsonHelper.JsonDeserialize<List<Project>>(Execute(request).Content);
        }

        public List<Node> GetWorkUnitNodes(int id)
        {
            var request = new RestRequest(Method.GET);
            request.Resource = "/api/v1/projects/work_unit_nodes";
            request.AddParameter("project_id", id);

            return JsonHelper.JsonDeserialize<List<Node>>(Execute(request).Content);
        }

        /// <summary>
        /// Function <c>ImportkpiEntriesAsync</c>
        /// </summary>
        /// <param name="entry">KpiEntry</param>
        /// <param name="callback">Callback</param>
        public void ImportKpiEntriesAsync(KpiEntry entry, Action<CIResponse> callback)
        {
            KpiEntryValidator validator = new KpiEntryValidator();
            List<KpiEntry> lst = new List<KpiEntry>();
            lst.Add(entry);
            validator.validate(lst);

            var request = new RestRequest(Method.POST);
            request.Resource = "/api/v1/kpi_entry/entry";

            //request.AddParameter("email", entry.Email);
            //request.AddParameter("kpi_id", entry.KpiID);
            //request.AddParameter("date", entry.Date);
            //request.AddParameter("value", entry.Value);
            request.AddParameter("entry", entry.toJson());

            ExecuteAsync(request, callback);
        }

        /// <summary>
        /// function ImportkpiEntriesAsync
        /// </summary>
        /// <param name="entries">array kpientries</param>
        /// <remarks>length of entries should not bigger than 500</remarks>
        /// <param name="callback">callback(CIResponse)</param>
        /// <param name="batch">batch</param>
        /// <remarks>
        /// default false,if true,server will rollback all the kpi entries if one couse error.
        /// if false,correct kpi entries will be insert into db and return the errors.
        /// </remarks>
        public void ImportKpiEntriesAsync(KpiEntry[] entries, Action<CIResponse> callback,bool batch = false)
        {
            if (entries.Length > (int)CIRequest.MAXKPIENTRYCOUNT)
            {
                throw new CiRequestTooLong("Maximum count of kpi entries is" + CIRequest.MAXKPIENTRYCOUNT);
            }

            KpiEntryValidator validator = new KpiEntryValidator();
            validator.validate(entries.OfType<KpiEntry>().ToList());

            var request = new RestRequest(Method.POST);
            request.Resource = "/api/v1/kpi_entry/entries";
            request.RequestFormat = DataFormat.Json;
            
            /*object[] objs = new object[entries.Length];
            for (int i = 0; i < entries.Length; i++)
            {
                objs[i] = new { kpi_id = entries[i].KpiID, date = entries[i].Date, value = entries[i].Value, email = entries[i].Email };
            }*/

            JArray array = new JArray();
            for (int i = 0; i < entries.Length; i++)
            {
                array.Add(entries[i].toJsonObject());
            }
            //
            request.AddParameter("entries", array.ToString());
            request.AddParameter("in_batch", batch);
            ExecuteAsync(request,callback);
        }

        /// <summary>
        /// Upload one kpi
        /// </summary>
        /// <param name="entry">ClearInsight.Model.KpiEntry</param>
        /// <returns>CIResponse response</returns>
        public CIResponse ImportKpiEntries(KpiEntry entry) 
        {
            KpiEntryValidator validator = new KpiEntryValidator();
            List<KpiEntry> lst = new List<KpiEntry>();
            lst.Add(entry);
            validator.validate(lst);
            var request = new RestRequest(Method.POST);

            request.Resource = "/api/v1/kpi_entry/entry";

            //request.AddParameter("email", entry.Email);
            //request.AddParameter("kpi_id", entry.KpiID);
            //request.AddParameter("date", entry.Date);
            //request.AddParameter("value", entry.Value);
            //Console.WriteLine(entry.toJson());
            request.AddParameter("entry", entry.toJson());

            return Execute(request);
        }

        /// <summary>
        /// Upload bulk kpientries
        /// </summary>
        /// <param name="entries">Array of ClearInsight.Model.KpiEntry</param>
        /// <remarks>length of entries should not bigger than 500</remarks>
        /// <returns>CIResponse response</returns>
        /// <param name="batch">batch</param>
        /// <remarks>
        /// default false,if true,server will rollback all the kpi entries if one couse error.
        /// if false,correct kpi entries will be insert into db and return the errors.
        /// </remarks>
        public CIResponse ImportKpiEntries(KpiEntry[] entries,bool batch = false)
        {
            if (entries.Length > (int)CIRequest.MAXKPIENTRYCOUNT)
            {
                throw new CiRequestTooLong("Maximum count of kpi entries is"+CIRequest.MAXKPIENTRYCOUNT);
            }
            KpiEntryValidator validator = new KpiEntryValidator();
            validator.validate(entries.OfType<KpiEntry>().ToList());

            var request = new RestRequest(Method.POST);
            request.Resource = "/api/v1/kpi_entry/entries";
            request.RequestFormat = DataFormat.Json;
            object[] objs = new object[entries.Length];
            /*for (int i = 0; i < entries.Length; i++)
            {
                objs[i] = new { kpi_id = entries[i].KpiID, date = entries[i].Date, value = entries[i].Value, email = entries[i].Email };
            }*/
            JArray array = new JArray();
            for (int i = 0; i < entries.Length; i++)
            {
                array.Add(entries[i].toJsonObject());
            }

            string temp = array.ToString();//request.JsonSerializer.Serialize(objs);
            request.AddParameter("entries", temp);
            request.AddParameter("in_batch", batch);
            return Execute(request);
        }

        /// <summary>
        /// A Test function
        /// </summary>
        /// <returns></returns>
        public CIResponse TestSecret()
        {
            var request = new RestRequest(Method.GET);
            request.Resource = "/api/v1/kpi_entry/secret";

            return Execute(request);
        }

        /// <summary>
        /// process the server response
        /// </summary>
        /// <param name="response">RestSharp.IRestResponse</param>
        /// <returns>CIResponse response</returns>
        private CIResponse _processStatusCode(IRestResponse response)
        {
            int statusCode = (int)response.StatusCode;
            CIResponse res = new CIResponse();
            res.Code = statusCode;
            res.Content = response.Content;

            //check defined msg
            switch (statusCode)
            {
                case (int)CIResponseCode.ArgumentError:
                    throw new CiArgumentErrorException(res.Content);
                default:
                    break;
            }
            return res;
        }
    }
}
