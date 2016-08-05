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
using System.Net;

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

            if (response.ErrorException != null)
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
        private void ExecuteAsync(RestRequest request, Action<CIResponse> callback)
        {
            var client = new RestClient();
            client.AddHandler("application.json", new JsonDeserializer());
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

            var response = Execute(request);

            if (response.Result) {
                return JsonHelper.JsonDeserialize<Msg<User>>(response.Content);
            } 
            else 
            {
                Msg<User> msg = new Msg<User>() { result = response.Result, content = response.ErrorMsg };
                return msg;
            }
        }

        /// <summary>
        /// User Logout
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns>CIResponse response</returns>
        public CIResponse UserLogout(string email, string password)
        {
            var request = new RestRequest(Method.POST);

            request.Resource = "/api/v1/users/logout";
            request.AddParameter("user", new User() { email = email, password = password }.toJson());

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

            //var response = Execute(request);

            return JsonHelper.JsonDeserialize<List<Project>>(Execute(request).Content);
        }

        public List<Node> GetWorkUnitNodes(int id)
        {
            var request = new RestRequest(Method.GET);
            request.Resource = "/api/v1/projects/work_unit_nodes";
            request.AddParameter("project_id", id);

            return JsonHelper.JsonDeserialize<List<Node>>(Execute(request).Content);
        }

        public Msg<string> BindNodeDevise(int id, string deviseCode)
        {
            var request = new RestRequest(Method.PUT);
            request.Resource = "/api/v1/nodes/bind_devise";
            request.AddParameter("id", id);
            request.AddParameter("devise_code", deviseCode);

            return JsonHelper.JsonDeserialize<Msg<string>>(Execute(request).Content);

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
            res.Result = true;

            //check defined msg
            switch (statusCode)
            {
                case (int)CIResponseCode.ArgumentError:
                    throw new CiArgumentErrorException(res.Content);
                case (int)CIResponseCode.NoServerError:
                    res.ErrorMsg = response.ErrorMessage;
                    res.Result = false;
                    throw new CiConnectErrorException(res.ErrorMsg);
                case (int) CIResponseCode.Unauthorized:
                    res.ErrorMsg = "没有权限进行操作";
                    res.Result = false;
                    throw new CiUnauthorizedErrorException(res.ErrorMsg);
                case (int)CIResponseCode.ServerError:
                    res.ErrorMsg = "服务器错误";
                    res.Result = false;
                    throw new CiServerErrorException(res.ErrorMsg);
                default:
                    break;
            }
            return res;
        }
    }
}
