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

namespace AbsolCase.Areas.ClinetPortal.Controllers
{
    [ServiceFilter(typeof(ConfigureSession))]
    public class MessagesController : Controller
    {
        private readonly IConfiguration config;

        public MessagesController(IConfiguration config)
        {
            this.config = config;
        }
        public IActionResult Messages()
        {
            string userId = string.Empty;
            string userdto = HttpContext.Session.GetString("userData");
            UserDto userData = JsonConvert.DeserializeObject<UserDto>(userdto);
            if (userData != null)
                userId = userData.Id;

            SResponse resp = Fetch.GotoService("api", $"Message/GetMessagesByUserId?userId={userId}", "GET");
            if (resp.ErrorCode == 401)
                return RedirectToAction("Login", "Account", new { area = "Security" });

            if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
            {
                List<Message> messages = JsonConvert.DeserializeObject<List<Message>>(resp.Resp);
                if (messages != null && messages.Count > 0)
                {
                    foreach (var message in messages)
                    {
                        if (!string.IsNullOrEmpty(message.UserImagePath))
                            message.UserImagePath = $"{config.GetValue<string>("App:RemoteServerUrl")}{message.UserImagePath}";
                        else
                            message.UserImagePath = $"{config.GetValue<string>("App:RemoteServerUrl")}Images/blank-profile.png";
                    }
                }
                return View(messages);
            }
            else
                return View();
        }
    }
}
