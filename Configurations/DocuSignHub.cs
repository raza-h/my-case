using AbsolCase.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using static AbsolCase.Configuration.RequestSender;
using AspNetCoreHero.ToastNotification.Notyf.Models;

namespace AbsolCase.Configurations
{
    public class DocuSignHub
    {
        private static IHttpContextAccessor httpContextAccessor;
        private static IConfiguration config;
        public static void SetHttpContextAccessor(IHttpContextAccessor accessor, IConfiguration configuration)
        {
            httpContextAccessor = accessor;
            config = configuration;
        }


        public async Task<bool> GetTokken(string code)
        {
            try
            {
                string webBasePath = config.GetValue<string>("App:RemoteWebUrl");

                string clientId = "776af63d-ca72-46f3-b5fb-1515cca41d7b";
                string tokenEndpoint = "https://account-d.docusign.com/oauth/token";
                string clientSecret = "c0ffddd8-fa5b-4332-bd11-1575788790b9";
                //string redirectUri = "http://localhost:25601/Attorney/Integrations/Token";

                string redirectUri = webBasePath + "Attorney/Integrations/Token";
                HttpClient client = new HttpClient();

                var formContent = new FormUrlEncodedContent(new[]
                {
                new KeyValuePair<string, string>("code", code),
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("client_id", clientId),
                new KeyValuePair<string, string>("client_secret", clientSecret),
                new KeyValuePair<string, string>("redirect_uri", redirectUri)
                });

                HttpResponseMessage response = await client.PostAsync(tokenEndpoint, formContent);

                string responseContent = await response.Content.ReadAsStringAsync();
                var serializedToken = responseContent;
                // Deserialize the JSON response
                var responseObject = JsonConvert.DeserializeObject<dynamic>(responseContent);

                // Retrieve the token from the response
                string token = responseObject.access_token;
                string refreshToken = responseObject.access_token;

                httpContextAccessor.HttpContext.Session.SetString("docusigntokken", serializedToken);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
