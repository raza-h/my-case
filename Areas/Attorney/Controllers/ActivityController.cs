using AbsolCase.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static AbsolCase.Configuration.RequestSender;

namespace AbsolCase.Areas.Attorney.Controllers
{
    public class ActivityController : Controller
    {
        public IActionResult Index()
        {
            SResponse resp = Fetch.GotoService("api", "Activity/GetActivity", "GET");
            if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
            {
                List<AdminActivity> activities = JsonConvert.DeserializeObject<List<AdminActivity>>(resp.Resp);
                return View(activities);
            }
            else
                return View();
        }
    }
}
