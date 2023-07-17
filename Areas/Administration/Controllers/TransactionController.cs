using AbsolCase.Configurations;
using AbsolCase.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using static AbsolCase.Configuration.RequestSender;

namespace AbsolCase.Areas.Administration.Controllers
{
    [ServiceFilter(typeof(ConfigureSession))]
    public class TransactionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CustomerTransactions()
        {
            SResponse resp = Fetch.GotoService("api", "Transaction/GetCustomersTransactions", "GET");
            if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
            {
                List<Transactions> transactions = JsonConvert.DeserializeObject<List<Transactions>>(resp.Resp);
                foreach (var item in transactions)
                {
                    item.NumAmountCredit = Convert.ToDouble(item.Amount);
                    item.NumAmountDebit = Convert.ToDouble(0.00);
                    if (item.AccountName== "Sales")
                    {
                        item.ClosingBalance = Convert.ToString(item.NumAmountCredit - item.NumAmountDebit);
                    }
                    else
                    {
                        item.ClosingBalance= Convert.ToString(item.NumAmountDebit - item.NumAmountCredit);
                    }
                }
                return View(transactions);
            }
            else
                return View();
        }
    }
}
