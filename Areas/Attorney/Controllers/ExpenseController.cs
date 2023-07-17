using AbsolCase.Configurations;
using AbsolCase.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Rotativa.AspNetCore;
using System;
using System.Collections.Generic;
using System.IO;
using static AbsolCase.Configuration.RequestSender;

namespace AbsolCase.Areas.Attorney.Controllers
{
    [ServiceFilter(typeof(ConfigureSession))]
    public class ExpenseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddExpense()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddExpense(Expense expense)
        {
            try
            {
                if (expense.Image != null && expense.Image.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        expense.Image.CopyTo(ms);
                        expense.File = ms.ToArray();
                        expense.Image = null;
                    }
                }
                var body = JsonConvert.SerializeObject(expense);
                SResponse resp = Fetch.GotoService("api", "Expense/AddExpense", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ManageExpense");
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
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet]
        public IActionResult Transactions()
        {
            SResponse resp = Fetch.GotoService("api", $"Transaction/GetTransactions", "GET");
            if (resp.ErrorCode == 401)
                return RedirectToAction("Login", "Account", new { area = "Security" });

            if (resp.Status && !string.IsNullOrEmpty(resp.Resp))
            {
                List<Transactions> clientTransaction = JsonConvert.DeserializeObject<List<Transactions>>(resp.Resp);
                return View(clientTransaction);
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
        public IActionResult ManageExpense()
        {
            SResponse resp = Fetch.GotoService("api", $"Expense/GetExpenses", "GET");
            if (resp.ErrorCode == 401)
                return RedirectToAction("Login", "Account", new { area = "Security" });

            if (resp.Status && !string.IsNullOrEmpty(resp.Resp))
            {
                List<ExpenseViewModel> _resultModel = JsonConvert.DeserializeObject<List<ExpenseViewModel>>(resp.Resp);
                return View(_resultModel);
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

        public IActionResult DownloadExpense(string StaffName, string Amount, string Description,string CreatedDate,int id)
        {
            try
            {
                ExpenseViewModel model = new ExpenseViewModel();
                model.ClientName = StaffName;
                model.CreatedDate= CreatedDate;
                model.Id= id;
                model.Amount = Convert.ToInt32(Amount);
                model.Description = Description;
                var pdfResult = new ViewAsPdf(model);
                return pdfResult;

            }
            catch (Exception ex)
            {
                throw;
            }


        }
    }
}
