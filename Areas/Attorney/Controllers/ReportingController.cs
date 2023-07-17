using AbsolCase.Configurations;
using AbsolCase.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Rotativa.AspNetCore;
using System;
using System.Collections.Generic;
using static AbsolCase.Configuration.RequestSender;

namespace AbsolCase.Areas.Attorney.Controllers
{
    [ServiceFilter(typeof(ConfigureSession))]
    public class ReportingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ClientReceipt()
        {
            return View();
        }
        public IActionResult ExpenseReceipt()
        {
            return View();
        }
        [HttpPost]
        public JsonResult SetClientTransactionList(List<ClientTransactionViewModel> ClientTransaction)
        {
            var SerializeObjectList = JsonConvert.SerializeObject(ClientTransaction);
            HttpContext.Session.SetString("ClientTransactions", SerializeObjectList);
            return Json("ok");
        }
        [HttpPost]
        public JsonResult SetExpenseList(List<JsonExpenseViewModel> Expenses)
        {
            var SerializeObjectExpenseList = JsonConvert.SerializeObject(Expenses);
            HttpContext.Session.SetString("Expenses", SerializeObjectExpenseList);
            return Json("ok");
        }
        [HttpGet]
        public IActionResult GenerateReceipt()
        {
            string ClientTransactions = HttpContext.Session.GetString("ClientTransactions");
            List<ClientTransactionViewModel> ClientTransactionsList = JsonConvert.DeserializeObject<List<ClientTransactionViewModel>>(ClientTransactions);
            return new ViewAsPdf(ClientTransactionsList);
        }
        [HttpGet]
        public IActionResult GenerateExpensesReceipt()
        {
            string Expenses = HttpContext.Session.GetString("Expenses");
            List<JsonExpenseViewModel> ExpenseList = JsonConvert.DeserializeObject<List<JsonExpenseViewModel>>(Expenses);
            return new ViewAsPdf(ExpenseList);
        }
        [HttpGet]
        public IActionResult Ledger()
        {
            return View();
        }

        public IActionResult GetTransactions()
        {
            SResponse resp = Fetch.GotoService("api", $"Transaction/GetTransactionsBetweenDates", "GET");
            if (resp.ErrorCode == 401)
                return RedirectToAction("Login", "Account", new { area = "Security" });

            if (resp.Status && !string.IsNullOrEmpty(resp.Resp))
            {
                List<Transactions> clientTransaction = JsonConvert.DeserializeObject<List<Transactions>>(resp.Resp);
                return PartialView("~/Views/PartialViews/_Ledger.cshtml", clientTransaction);
            }
            else
                return View();
        }


        public IActionResult Reporting()
        {
            return View();
        }

        public IActionResult AllClientInvoices()
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", $"UserManagement/GetUsersClient", "GET");
                if (resp.ErrorCode == 401)
                    return RedirectToAction("Login", "Account", new { area = "Security" });

                if (resp.Status && !string.IsNullOrEmpty(resp.Resp))
                {
                    List<Contact> clients = JsonConvert.DeserializeObject<List<Contact>>(resp.Resp);
                    return View(clients);
                }
                else
                    return View();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public IActionResult AllCasesInvoices()
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", $"UserManagement/GetCasesClient", "GET");
                if (resp.ErrorCode == 401)
                    return RedirectToAction("Login", "Account", new { area = "Security" });

                if (resp.Status && !string.IsNullOrEmpty(resp.Resp))
                {
                    List<CaseDetail> caseDetails = JsonConvert.DeserializeObject<List<CaseDetail>>(resp.Resp);
                    return View(caseDetails);
                }
                else
                    return View();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
