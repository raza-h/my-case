using AbsolCase.Configurations;
using AbsolCase.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static AbsolCase.Configuration.RequestSender;

namespace AbsolCase.Areas.Staff.Controllers
{
    [ServiceFilter(typeof(ConfigureSession))]
    public class StaffTimelineController : Controller
    {
        private readonly IConfiguration config;
        public StaffTimelineController(IConfiguration config)
        {
            this.config = config;
        }
        public IActionResult TimeLine()
        {
            string userId = string.Empty;
            string userdto = HttpContext.Session.GetString("userData");
            UserDto userData = JsonConvert.DeserializeObject<UserDto>(userdto);
            if (userData != null)
                userId = userData.Id;
            SResponse resp = Fetch.GotoService("api", $"TimeLine/GetTimeLinesByUserIdStaff?userId={userId}", "GET");
            if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
            {
                List<Timeline> timelines = JsonConvert.DeserializeObject<List<Timeline>>(resp.Resp);
                if (timelines != null && timelines.Count > 0)
                {
                    foreach (var timeline in timelines)
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
                return View();
        }
    }
}
