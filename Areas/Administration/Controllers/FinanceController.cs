using AbsolCase.Configurations;
using AbsolCase.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static AbsolCase.Configuration.RequestSender;

namespace AbsolCase.Areas.Administration.Controllers
{
    [ServiceFilter(typeof(ConfigureSession))]
    public class FinanceController : Controller
    {
        public IActionResult Payments()
        {
            SResponse resp = Fetch.GotoService("api", $"FinancialDetails/GetPayments", "GET");
            if (resp.ErrorCode == 401)
                return RedirectToAction("Login", "Account", new { area = "Security" });

            if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
            {
                List<PaymentInfoDto> payments = JsonConvert.DeserializeObject<List<PaymentInfoDto>>(resp.Resp);
                return View(payments);
            }
            else
                return View();
        }
        public IActionResult PaymentDetail(int paymentId)
        {
            SResponse resp = Fetch.GotoService("api", $"FinancialDetails/PaymentDetail?paymentId={paymentId}", "GET");
            if (resp.ErrorCode == 401)
                return RedirectToAction("Login", "Account", new { area = "Security" });

            if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
            {
                PaymentInfoDto payment = JsonConvert.DeserializeObject<PaymentInfoDto>(resp.Resp);
                return View(payment);
            }
            else
                return View();
        }
    }
}
