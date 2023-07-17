using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AbsolCase.Configurations;
using AbsolCase.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static AbsolCase.Configuration.RequestSender;

namespace AbsolCase.Areas.Staff.Controllers
{
    [ServiceFilter(typeof(ConfigureSession))]
    public class CaseController : Controller
    {
        public IActionResult CasesList()
        {
            string userId = string.Empty;
            string userData = HttpContext.Session.GetString("userData");
            if (!string.IsNullOrEmpty(userData))
            {
                UserDto userDto = JsonConvert.DeserializeObject<UserDto>(userData);
                userId = userDto.Id;
            }
            SResponse resp = AbsolCase.Configuration.RequestSender.Fetch.GotoService("api", $"Case/GetCasesByStaffId?userId={userId}", "GET");
            if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
            {
                List<CaseDetail> cases = JsonConvert.DeserializeObject<List<CaseDetail>>(resp.Resp);
                return View(cases);
            }
            return View();
        }
    }
}
