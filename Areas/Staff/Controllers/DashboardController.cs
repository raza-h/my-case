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

namespace AbsolCase.Areas.Staff.Controllers
{
    [ServiceFilter(typeof(ConfigureSession))]
    public class DashboardController : Controller
    {
        public IActionResult Dashboard()
        {
            string userId = string.Empty;
            string userdto = HttpContext.Session.GetString("userData");
            UserDto userData = JsonConvert.DeserializeObject<UserDto>(userdto);
            if (userData != null)
                userId = userData.Id;

            SResponse resp = AbsolCase.Configuration.RequestSender.Fetch.GotoService("api", $"Case/GetCasesByStaffId?userId={userId}", "GET");
            if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
            {
                List<CaseDetail> cases = JsonConvert.DeserializeObject<List<CaseDetail>>(resp.Resp);
                ViewBag.Cases = cases;
            }

            SResponse response = AbsolCase.Configuration.RequestSender.Fetch.GotoService("api", "News/GetAllNews", "GET");
            if (response.Status && (response.Resp != null) && (response.Resp != ""))
            {
                List<News> ListOfNews = JsonConvert.DeserializeObject<List<News>>(response.Resp);
                ViewBag.News = ListOfNews.ToList().Where(x => x.SendTo == "AttorneyToClient").ToList();
                return View();
            }
            return View();
        }
    }
}
