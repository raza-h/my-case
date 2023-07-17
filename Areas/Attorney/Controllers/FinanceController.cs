using AbsolCase.Configurations;
using AbsolCase.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static AbsolCase.Configuration.RequestSender;

namespace AbsolCase.Areas.Attorney.Controllers
{
    [ServiceFilter(typeof(ConfigureSession))]
    public class FinanceController : Controller
    {
        public IActionResult Index()
        {
            SResponse resp = Fetch.GotoService("api", $"FinancialDetails/GetFinancialDetails", "GET");
            if (resp.ErrorCode == 401)
                return RedirectToAction("Login", "Account", new { area = "Security" });

            if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
            {
                List<FinancialDetails> users = JsonConvert.DeserializeObject<List<FinancialDetails>>(resp.Resp);
                return View(users);
            }
            else
                return View();
        }
        public IActionResult Invoices()
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
        public IActionResult InvoiceDetail(int paymentId)
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
        public IActionResult PaymentReceived()
        {
                return View();
        }
        public IActionResult AddReceipt()
        {
            string userdto = HttpContext.Session.GetString("userData");
            UserDto userData = JsonConvert.DeserializeObject<UserDto>(userdto);

            SResponse resp = Fetch.GotoService("api", "Contact/GetContacts", "GET");
            if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
            {
                List<Contact> contacts = JsonConvert.DeserializeObject<List<Contact>>(resp.Resp).ToList();
                ViewBag.Clients = new SelectList(contacts, "ContactId", "FirstName");
            }
            else
            {
                List<Contact> contacts = new List<Contact>();
                ViewBag.Clients = new SelectList(contacts, "ContactId", "FirstName");

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
            SResponse response = Fetch.GotoService("api", $"UserManagement/GetUsers?ParentId={userData.Id}", "GET");
            if (response.Status && !string.IsNullOrEmpty(response.Resp))
            {
                List<UserDto> users = JsonConvert.DeserializeObject<List<UserDto>>(response.Resp).ToList();
                ViewBag.Users = new SelectList(users, "Id", "FirstName");
            }
            else
            {
                List<UserDto> users = new List<UserDto>();
                ViewBag.Users = new SelectList(users, "Id", "FirstName");


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
        [HttpPost]
        public IActionResult AddReceipt(ClientTransaction clientTransaction)
        {
            try
            {
                if (clientTransaction.Image != null && clientTransaction.Image.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        clientTransaction.Image.CopyTo(ms);
                        clientTransaction.File = ms.ToArray();
                        clientTransaction.Image = null;
                    }
                }
                var body = JsonConvert.SerializeObject(clientTransaction);
                SResponse resp = Fetch.GotoService("api", "ClientTransaction/AddClientTransaction", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ClientTransaction");
                }
                else
                {
                    TempData["response"] = resp.Resp;
                    return RedirectToAction("AddReceipt");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        public IActionResult UpdateReceipt(int Id)
        {
            string userdto = HttpContext.Session.GetString("userData");
            UserDto userData = JsonConvert.DeserializeObject<UserDto>(userdto);

            SResponse resp = Fetch.GotoService("api", "Contact/GetContacts", "GET");
            if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
            {
                List<Contact> contacts = JsonConvert.DeserializeObject<List<Contact>>(resp.Resp).ToList();
                ViewBag.Clients = new SelectList(contacts, "ContactId", "FirstName");
            }
            SResponse response = Fetch.GotoService("api", $"UserManagement/GetUsers?ParentId={userData.Id}", "GET");
            if (response.Status && !string.IsNullOrEmpty(response.Resp))
            {
                List<UserDto> users = JsonConvert.DeserializeObject<List<UserDto>>(response.Resp).ToList();
                ViewBag.Users = new SelectList(users, "Id", "FirstName");
            }
            SResponse getReceipt = Fetch.GotoService("api", $"ClientTransaction/GetTransactionById?Id={Id}", "GET");
            if (getReceipt.Status && !string.IsNullOrEmpty(getReceipt.Resp))
            {
                ClientTransaction transaction = JsonConvert.DeserializeObject<ClientTransaction>(getReceipt.Resp);
                transaction.CheckDate = transaction.CheckDate != null ? transaction.CheckDate.Value.Date : null;
                return View(transaction);
            }
            return View();
        }
        [HttpPost]
        public IActionResult UpdateReceipt(ClientTransaction clientTransaction)
        {
            try
            {
                if (clientTransaction.Image != null && clientTransaction.Image.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        clientTransaction.Image.CopyTo(ms);
                        clientTransaction.File = ms.ToArray();
                        clientTransaction.Image = null;
                    }
                }
                var body = JsonConvert.SerializeObject(clientTransaction);
                SResponse resp = Fetch.GotoService("api", "ClientTransaction/UpdateClientTransaction", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ClientTransaction");
                }
                else
                {
                    TempData["response"] = resp.Resp + " " + "Unable To Add Receipt";
                    return RedirectToAction("AddReceipt");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IActionResult ClientTransaction()
        {
            string ParentId = string.Empty;
            string userData = HttpContext.Session.GetString("userData");
            if (!string.IsNullOrEmpty(userData))
            {
                UserDto userDto = JsonConvert.DeserializeObject<UserDto>(userData);
                ParentId = userDto.Id;
            }
            SResponse resp = Fetch.GotoService("api", $"ClientTransaction/GetTransactions?ParentId={ParentId}", "GET");
            if (resp.ErrorCode == 401)
                return RedirectToAction("Login", "Account", new { area = "Security" });

            if (resp.Status && !string.IsNullOrEmpty(resp.Resp))
            {
                List<ClientTransaction> clientTransaction = JsonConvert.DeserializeObject<List<ClientTransaction>>(resp.Resp);
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

        public IActionResult Subscriptions()
        {
            SResponse resp = Fetch.GotoService("api", $"FinancialDetails/GetSubscriptionNames", "GET");
            if (resp.ErrorCode == 401)
                return RedirectToAction("Login", "Account", new { area = "Security" });

            if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
            {
                List<Service> serviceNames = JsonConvert.DeserializeObject<List<Service>>(resp.Resp);
                return View(serviceNames);
            }
            else
                return View();
        }


    }
}
