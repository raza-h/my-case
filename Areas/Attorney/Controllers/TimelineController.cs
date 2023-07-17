using AbsolCase.Configurations;
using AbsolCase.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PayPal.Api;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static AbsolCase.Configuration.RequestSender;

namespace AbsolCase.Areas.Attorney.Controllers  
{
    [ServiceFilter(typeof(ConfigureSession))]
    public class TimelineController : Controller
    {
        private readonly IConfiguration config;
        public TimelineController(IConfiguration config)
        {
            this.config = config;
        }
        public IActionResult Index()
        {
            string userId = string.Empty;
            string userdto = HttpContext.Session.GetString("userData");
            UserDto userData = JsonConvert.DeserializeObject<UserDto>(userdto);
            if (userData != null)
                userId = userData.Id;
            SResponse resp = Fetch.GotoService("api", $"TimeLine/GetTimeLinesByUserId?userId={userId}", "GET");
            if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
            {
                List<Timeline> timelines = JsonConvert.DeserializeObject<List<Timeline>>(resp.Resp);
                if(timelines != null && timelines.Count > 0)
                {
                    foreach( var timeline in timelines)
                    {
                        if (!string.IsNullOrEmpty(timeline.FilePath) && !string.IsNullOrEmpty(timeline.FilePath.Trim()))
                            timeline.FilePath = $"{config.GetValue<string>("App:RemoteServerUrl")}{timeline.FilePath}";
                        if (!string.IsNullOrEmpty(timeline.DocFilePath) && !string.IsNullOrEmpty(timeline.DocFilePath.Trim()))
                            timeline.DocFilePath = $"{config.GetValue<string>("App:RemoteServerUrl")}{timeline.DocFilePath}";
                        if (!string.IsNullOrEmpty(timeline.VideoFilePath) && !string.IsNullOrEmpty(timeline.VideoFilePath.Trim()))
                            timeline.VideoFilePath = $"{config.GetValue<string>("App:RemoteServerUrl")}{timeline.VideoFilePath}";
                        if (!string.IsNullOrEmpty(timeline.UserImagePath) && !string.IsNullOrEmpty(timeline.UserImagePath.Trim()))
                            timeline.UserImagePath = $"{config.GetValue<string>("App:RemoteServerUrl")}{timeline.UserImagePath}";
                        else
                            timeline.UserImagePath = $"{config.GetValue<string>("App:RemoteServerUrl")}Images/blank-profile.png";
                    }
                }
                return View(timelines);
            }
            else
            {
                if (resp.Resp == "Please Add Firm Details First")
                {
                    TempData["response"] = "Please Add Firm Details First";
                    return RedirectToAction("AddFirm", "Firm", new { area = "Attorney" });
                }
                else
                {
                    TempData["response"] = resp.Resp;
                    return View();
                }
            }
            return View();
        }

        //public IActionResult Zoom()
        //{
        //    var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
        //    var now = DateTime.UtcNow;
        //    //var apiSecret = "Your API secret";
        //    var apiSecret = "rJwQEE1j2N4XT8GwsBEvYCj371FHZUnRmtSS";
        //    byte[] symmetricKey = Encoding.ASCII.GetBytes(apiSecret);





        //    var tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        //Issuer = "Your API Key",
        //        Issuer = "ZwW8_Kl9TvaTb5piRa_4Yg",
        //        Expires = now.AddSeconds(300),
        //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256),
        //    };
        //    var token = tokenHandler.CreateToken(tokenDescriptor);
        //    var tokenString = tokenHandler.WriteToken(token);





        //    var client = new RestClient("https://api.zoom.us/v2/users/haseeb.bsse3001@iiu.edu.pk/meetings");
        //    var request = new RestRequest(Method.POST);
        //    request.RequestFormat = DataFormat.Json;
        //    request.AddJsonBody(new { topic = "Meeting with Ussain", duration = "10", start_time = "2021-05-20T05:00:00", type = "2" });
        //    request.AddHeader("authorization", String.Format("Bearer {0}", tokenString));



        //    IRestResponse restResponse = client.Execute(request);
        //    HttpStatusCode statusCode = restResponse.StatusCode;
        //    int numericStatusCode = (int)statusCode;
        //    var jObject = JObject.Parse(restResponse.Content);
        //    Zoom zoomModel = new Zoom();
        //    zoomModel.Host = (string)jObject["start_url"];
        //    zoomModel.Join = (string)jObject["join_url"];
        //    zoomModel.Code = Convert.ToString(numericStatusCode);

        //    return View(zoomModel);
        //}

        //[HttpPost]
        //public IActionResult Zoomhaseebuser(Timeline timeline)
        //{
        //    var starttime = timeline.starttime;
        //    var duration = timeline.duration;
        //    var topic = timeline.topic;
        //    var timezone = timeline.timezone;
        //    if (timezone == "1")
        //    {
        //        timezone = "Asia/Tashkent";
        //    }
        //    else if (timezone == "2")
        //    {
        //        timezone = "Asia/Riyadh";
        //    }
        //    else if (timezone == "3")
        //    {
        //        timezone = "America/Los_Angeles";
        //    }
        //    //string dateTime =Convert.ToDateTime(starttime);
        //    starttime = starttime + ":00Z";
        //    var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
        //    var now = DateTime.UtcNow;
        //    //var apiSecret = "Your API secret";
        //    var apiSecret = "rJwQEE1j2N4XT8GwsBEvYCj371FHZUnRmtSS";
        //    byte[] symmetricKey = Encoding.ASCII.GetBytes(apiSecret);





        //    var tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        //Issuer = "Your API Key",
        //        Issuer = "ZwW8_Kl9TvaTb5piRa_4Yg",
        //        Expires = now.AddSeconds(300),
        //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256),
        //    };
        //    var token = tokenHandler.CreateToken(tokenDescriptor);
        //    var tokenString = tokenHandler.WriteToken(token);





        //    var client = new RestClient("https://api.zoom.us/v2/users/haseeb.bsse3001@iiu.edu.pk/meetings");
        //    var request = new RestRequest(Method.POST);
        //    request.RequestFormat = DataFormat.Json;
        //    //request.AddJsonBody(new { topic = "Meeting with Ussain", agenda="agenda", duration = "10", start_time = "2021-05-20T05:00:00", type = "2" });
        //    request.AddJsonBody(new { topic = topic, duration = duration, start_time = starttime, auto_recording = "local", jbh_time = "0", join_before_host = "true", timezone = timezone, type = "2" });
        //    request.AddHeader("authorization", String.Format("Bearer {0}", tokenString));



        //    IRestResponse restResponse = client.Execute(request);
        //    HttpStatusCode statusCode = restResponse.StatusCode;
        //    int numericStatusCode = (int)statusCode;
        //    var jObject = JObject.Parse(restResponse.Content);
        //    Zoom zoomModel = new Zoom();
        //    zoomModel.Host = (string)jObject["start_url"];
        //    zoomModel.Join = (string)jObject["join_url"];
        //    zoomModel.Code = Convert.ToString(numericStatusCode);

        //    return Json(zoomModel);
        //}
        [HttpPost]
        public IActionResult Zoom(Timeline timeline)
        {
            var starttime = timeline.starttime;
            var duration = timeline.duration;
            var topic = timeline.topic;
            var timezone = timeline.timezone;
            if (timezone == "1")
            {
                timezone = "Asia/Tashkent";
            }
            else if (timezone == "2")
            {
                timezone = "Asia/Riyadh";
            }
            else if (timezone == "3")
            {
                timezone = "America/Los_Angeles";
            }
            //string dateTime =Convert.ToDateTime(starttime);
            starttime = starttime + ":00Z";
            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var now = DateTime.UtcNow;
            //var apiSecret = "Your API secret";
            var apiSecret = "QBBiGJkMGiAFuBEdUiXrEO4YxgIpFADB19Vs";
            byte[] symmetricKey = Encoding.ASCII.GetBytes(apiSecret);





            var tokenDescriptor = new SecurityTokenDescriptor
            {
                //Issuer = "Your API Key",
                Issuer = "Wa--b6UgQMGDrlDO3ZFoJg",
                Expires = now.AddSeconds(300),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256),
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);





            var client = new RestClient("https://api.zoom.us/v2/users/absolzoom@gmail.com/meetings");
            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;
            //request.AddJsonBody(new { topic = "Meeting with Ussain", agenda="agenda", duration = "10", start_time = "2021-05-20T05:00:00", type = "2" });
            request.AddJsonBody(new { topic = topic, duration = duration, start_time = starttime, auto_recording = "local", jbh_time = "0", join_before_host = "true", timezone = timezone, type = "2" });
            request.AddHeader("authorization", String.Format("Bearer {0}", tokenString));



            IRestResponse restResponse = client.Execute(request);
            HttpStatusCode statusCode = restResponse.StatusCode;
            int numericStatusCode = (int)statusCode;
            var jObject = JObject.Parse(restResponse.Content);
            Zoom zoomModel = new Zoom();
            zoomModel.Host = (string)jObject["start_url"];
            zoomModel.Join = (string)jObject["join_url"];
            zoomModel.Code = Convert.ToString(numericStatusCode);

            return Json(zoomModel);
        }

        [HttpGet]
        public IActionResult ZoomMeetingGet()
        {
            
            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var now = DateTime.UtcNow;
            //var apiSecret = "Your API secret";
            var apiSecret = "QBBiGJkMGiAFuBEdUiXrEO4YxgIpFADB19Vs";
            byte[] symmetricKey = Encoding.ASCII.GetBytes(apiSecret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                //Issuer = "Your API Key",
                Issuer = "Wa--b6UgQMGDrlDO3ZFoJg",
                Expires = now.AddSeconds(300),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256),
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            var from= DateTime.Now.AddDays(-14).ToString("yyyy-MM-dd");
            //var to = DateTime.Now.AddDays(+3).ToString("yyyy-MM-dd");
            var to = DateTime.Now.ToString("yyyy-MM-dd");
            var client = new RestClient("https://api.zoom.us/v2/users/absolzoom@gmail.com/recordings?page_size=30&from="+ from + "&to="+ to + "");
           

            var request = new RestRequest(Method.GET);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("authorization", String.Format("Bearer {0}", tokenString));



            IRestResponse restResponse = client.Execute(request);
            HttpStatusCode statusCode = restResponse.StatusCode;
            int numericStatusCode = (int)statusCode;
            var jObjectdata = JObject.Parse(restResponse.Content);



            List<Zoom> zoomModel = new List<Zoom>();
           
            var data=jObjectdata.ToString();

            var newdata = JsonConvert.DeserializeObject<Meetingresult>(data);
            
            for (int i = 0; i < newdata.meetings.Count(); i++)
            {
                Zoom obj = new Zoom();
                //var stringg = Convert.ToString(newdata.meetings[i].share_url);
                //zoomModel[i].shareurl = stringg;
                obj.meetingId = newdata.meetings[i].id;

                var tokenHandlerr = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                var noww = DateTime.UtcNow;
                //var apiSecret = "Your API secret";
                var apiSecrett = "QBBiGJkMGiAFuBEdUiXrEO4YxgIpFADB19Vs";
                byte[] symmetricKeyy = Encoding.ASCII.GetBytes(apiSecrett);
                var tokenDescriptorr = new SecurityTokenDescriptor
                {
                    //Issuer = "Your API Key",
                    Issuer = "Wa--b6UgQMGDrlDO3ZFoJg",
                    Expires = noww.AddSeconds(300),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKeyy), SecurityAlgorithms.HmacSha256),
                };
                var tokenn = tokenHandlerr.CreateToken(tokenDescriptorr);
                var tokenStringg = tokenHandlerr.WriteToken(tokenn);

                var getmeeting = new RestClient("https://api.zoom.us/v2/meetings/" + obj.meetingId + "/recordings");
                //var getmeeting = new RestClient("https://api.zoom.us/v2/meetings/86537176557/recordings");
                var newrequest = new RestRequest(Method.GET);
                newrequest.RequestFormat = DataFormat.Json;
                newrequest.AddHeader("authorization", String.Format("Bearer {0}", tokenStringg));
                IRestResponse restResponsee = getmeeting.Execute(newrequest);
                var getmeetingdata = JObject.Parse(restResponsee.Content);
                var meetinggetdata = getmeetingdata.ToString();
                var meetingDetails = JsonConvert.DeserializeObject<meetingDetails>(meetinggetdata);

                obj.shareurl = meetingDetails.share_url;
                obj.passcode = meetingDetails.password;
                obj.title=meetingDetails.topic;

                zoomModel.Add(obj);
            }
            

            

            return Json(zoomModel);
        }

    }
}
