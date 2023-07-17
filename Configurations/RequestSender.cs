using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using AbsolCase.Models;
using AspNetCoreHero.ToastNotification.Notyf.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace AbsolCase.Configuration
{
    public class RequestSender
    {
        private static IHttpContextAccessor httpContextAccessor;
        private static IConfiguration _config;

        public static void SetIHttpContextAccessor(IHttpContextAccessor accessor, IConfiguration config)
        {
            httpContextAccessor = accessor;
            _config = config;
        }

        public static RequestSender Fetch
        {
            get
            {
                if (fetch == null)
                {
                    fetch = new RequestSender();
                }
                return fetch;
            }
        }
        private static RequestSender fetch;
        private RequestSender(){}
        public SResponse GotoService(string otype, string action, string method, string inpobject = "")
        {
            string AccessToken = string.Empty;
            string userData = httpContextAccessor.HttpContext.Session.GetString("userData");
            if (!string.IsNullOrEmpty(userData))
            {
                UserDto userDto = JsonConvert.DeserializeObject<UserDto>(userData);
                AccessToken = userDto.Token;
            }
            string apiBasePath = _config.GetValue<string>("App:RemoteServerUrl");
            SResponse waresp = new SResponse();
            string resp = "";
            try
            {
                ////https://apps.ab-sol.net/absolcaseapi/
                // https://localhost:44318/
                //http://199.231.160.216/cms-apiqa/
                //http://108.60.206.3/cmsapi/
                //http://173.208.142.67:5955/cmsapi/

                //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://38.17.51.206:8010/" + otype + "/" + action);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiBasePath + otype + "/" + action);
                request.Headers.Add("Authorization", "Bearer " + AccessToken);
                request.Credentials = CredentialCache.DefaultCredentials;
                request.Method = method;
                if (System.Diagnostics.Debugger.IsAttached)
                    request.Timeout = 120000;
                else
                    request.Timeout = 40000;

                request.ContentType = "application/json";
                request.ContentLength = 0;

                if (inpobject != "")
                {
                    byte[] byteData = Encoding.UTF8.GetBytes(inpobject);
                    request.ContentLength = byteData.Length;
                    Stream stream = request.GetRequestStream();
                    stream.Write(byteData, 0, byteData.Length);
                    stream.Close();
                }
                ServicePointManager.ServerCertificateValidationCallback =
                new RemoteCertificateValidationCallback(
                delegate (
                object sender2,
                X509Certificate certificate,
                X509Chain chain,
                SslPolicyErrors sslPolicyErrors)
                {
                    return true;
                });
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream());
                resp = sr.ReadToEnd();
                waresp.Status = true;
                waresp.Resp = resp;
                return waresp;
            }
            catch (WebException wex)
            {
                if (wex != null && wex.Response != null)
                {
                    StreamReader sr = new StreamReader(wex.Response.GetResponseStream());
                    waresp.Resp = sr.ReadToEnd();
                }
                else
                    waresp.Resp = wex.Message;
                waresp.Status = false;
                waresp.Error = wex.Message;
                if (wex.Message == "error: (403) Forbidden.")
                    waresp.ErrorCode = 403;
                else if (wex.Message == "The remote server returned an error: (401) Unauthorized.")
                    waresp.ErrorCode = 401;
                else
                    waresp.ErrorCode = 0;
                return waresp;
            }
            catch (Exception ex)
            {
                waresp.Status = false;
                waresp.Error = ex.Message;
                waresp.ErrorCode = 0;
                return waresp;
            }

            finally
            {

            }
        }


        public class SResponse
        {
            public SResponse() { }

            public string Error { get; set; }
            public int ErrorCode { get; set; }
            public string Key { get; set; }
            public string msg { get; set; }
            public string Resp { get; set; }
            public bool Status { get; set; }

            public enum RequestMethod
            {
                GET,
                POST,
                DELETE,
                PUT,
            }
        }
    }
}