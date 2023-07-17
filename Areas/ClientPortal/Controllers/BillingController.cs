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
    public class BillingController : Controller
    {
        public IActionResult Index()
        {
            string userId = string.Empty;
            string userdto = HttpContext.Session.GetString("userData");
            UserDto userData = JsonConvert.DeserializeObject<UserDto>(userdto);
            if(userData != null)
                userId = userData.ParentId;
            SResponse resp = Fetch.GotoService("api", $"ClientTransaction/GetTransactionsByUserId?userId={userId}", "GET");
            if (resp.ErrorCode == 401)
                return RedirectToAction("Login", "Account", new { area = "Security" });

            if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
            {
                List<ClientTransaction> transactions = JsonConvert.DeserializeObject<List<ClientTransaction>>(resp.Resp);
                return View(transactions);
            }
            else
                return View();
        }
    }
}
