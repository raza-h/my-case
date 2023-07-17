using AbsolCase.Configurations;
using AbsolCase.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static AbsolCase.Configuration.RequestSender;

namespace AbsolCase.Areas.ClinetPortal.Controllers
{
    [ServiceFilter(typeof(ConfigureSession))]
    public class TasksController : Controller
    {
        public IActionResult Tasks()
        {
            string ClientId = string.Empty;
            string userData = HttpContext.Session.GetString("userData");
            if (!string.IsNullOrEmpty(userData))
            {
                UserDto userDto = JsonConvert.DeserializeObject<UserDto>(userData);
                ClientId = userDto.Id;
            }
            SResponse taskresp = Fetch.GotoService("api", $"Task/GetTasksClient?userId={ClientId}&&userType=client", "GET");
            if (taskresp.Status && (taskresp.Resp != null) && (taskresp.Resp != ""))
            {
                List<Tasks> task = JsonConvert.DeserializeObject<List<Tasks>>(taskresp.Resp);
                return View(task);
            }
            else
                return View();
        }
        public IActionResult ViewDetail(int Id)
        {
            SResponse taskresp = Fetch.GotoService("api", $"Task/GetTaskById?Id={Id}", "GET");
            if (taskresp.Status && (taskresp.Resp != null) && (taskresp.Resp != ""))
            {
                Tasks task = JsonConvert.DeserializeObject<Tasks>(taskresp.Resp);
                return PartialView("~/Views/PartialViews/_TaskDetail.cshtml", task);
            }
            else
                return PartialView("~/Views/PartialViews/_TaskDetail.cshtml");
        }
    }
}
