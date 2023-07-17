using AbsolCase.Configurations;
using AbsolCase.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static AbsolCase.Configuration.RequestSender;
namespace AbsolCase.Areas.Cases.Controllers
{
    [ServiceFilter(typeof(ConfigureSession))]
    public class CompanyController : Controller
    {
        private readonly ISession session;
        public CompanyController(IHttpContextAccessor httpContextAccessor)
        {
            this.session = httpContextAccessor.HttpContext.Session;
        }
        public IActionResult AddCompany()
        {
            string userId = string.Empty;
            string userdto = HttpContext.Session.GetString("userData");
            if (!string.IsNullOrEmpty(userdto))
            {
                UserDto userData = JsonConvert.DeserializeObject<UserDto>(userdto);
                userId = userData.Id;
            }
            SResponse resp = Fetch.GotoService("api", $"Firm/GetFirmByUserId?userId={userId}", "GET");
            if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
            {
                return View();
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
        public IActionResult ManageCompany()
        {
            SResponse resp = Fetch.GotoService("api", "CompanyManagement/GET", "GET");
            if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
            {
                List<Company> company = JsonConvert.DeserializeObject<List<Company>>(resp.Resp);
                return View(company);
            }
            else
            {
                if(resp.Resp == "Please Add Firm Details First")
                {
                    return RedirectToAction("AddFirm", "Firm", new { area = "Attorney"});
                }
            }
                return View();
        }
        public IActionResult UpdateCompany(int Id)
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", $"CompanyManagement/GetById?Id={Id}", "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {

                    Company company = JsonConvert.DeserializeObject<Company>(resp.Resp);
                    return View(company);
                }
                else
                    return View();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpPost]
        public IActionResult UpdateCompany(Company company)
        {
            try
            {
                var body = JsonConvert.SerializeObject(company);
                SResponse resp = Fetch.GotoService("api", "CompanyManagement/Update", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ManageCompany");
                }
                else
                {
                    TempData["response"] = resp.Resp;
                    return View();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IActionResult DeleteCompany(int Id)
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", $"CompanyManagement/Delete?Id={Id}", "DELETE");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ManageCompany");
                }
                return RedirectToAction("ManageCompany");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public IActionResult AddContact()
        {
            string userId = string.Empty;
            string userdto = HttpContext.Session.GetString("userData");
            if (!string.IsNullOrEmpty(userdto))
            {
                UserDto userData = JsonConvert.DeserializeObject<UserDto>(userdto);
                userId = userData.Id;
            }
            SResponse resp = Fetch.GotoService("api", $"Firm/GetFirmByUserId?userId={userId}", "GET");
            if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
            {
                return View();
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
        public IActionResult ManageContact()
        {
            SResponse resp = Fetch.GotoService("api", "Contact/GetContacts", "GET");
            if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
            {
                List<Contact> contact = JsonConvert.DeserializeObject<List<Contact>>(resp.Resp);
                return View(contact);
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
        public IActionResult UpdateContact(int Id)
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", $"Contact/GetById?Id={Id}", "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {

                    Contact contact = JsonConvert.DeserializeObject<Contact>(resp.Resp);
                    return View(contact);
                }
                else
                    return View();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpPost]
        public IActionResult UpdateContact(Contact contact)
        {
            try
            {
                var body = JsonConvert.SerializeObject(contact);
                SResponse resp = Fetch.GotoService("api", "Contact/Update", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ManageContact");
                }
                else
                {
                    TempData["response"] = resp.Resp;
                    return View();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IActionResult DeleteContact(int Id)
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", $"Contact/Delete?Id={Id}", "DELETE");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ManageContact");
                }
                return RedirectToAction("ManageContact");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public IActionResult AddCase()
        {
            string userId = string.Empty;
            string userdto = HttpContext.Session.GetString("userData");
            if (!string.IsNullOrEmpty(userdto))
            {
                UserDto userData = JsonConvert.DeserializeObject<UserDto>(userdto);
                userId = userData.Id;
            }

            SResponse resp = Fetch.GotoService("api", $"Firm/GetFirmByUserId?userId={userId}", "GET");
            if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
            {
                return View();
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

            /// For user Atterny Drop Down.....
            SResponse responseFOrUser = Fetch.GotoService("api", $"UserManagement/GetUsers?ParentId={userId}", "GET");
            if (responseFOrUser.Status && (responseFOrUser.Resp != null) && (responseFOrUser.Resp != ""))
            {
                List<AspNetUsers> LeadAttorney = JsonConvert.DeserializeObject<List<AspNetUsers>>(responseFOrUser.Resp);
                ViewBag.LeadAttorney = new SelectList(LeadAttorney, "Id", "FirstName");
            }
            else
            {
                if (resp.Resp == "Please Add Firm Details First")
                {
                    List<AspNetUsers> LeadAttorney = new List<AspNetUsers>();
                    ViewBag.LeadAttorney = null;
                }
                else
                {
                    TempData["response"] = resp.Resp;
                    List<AspNetUsers> LeadAttorney = new List<AspNetUsers>();
                    ViewBag.LeadAttorney = null;// LeadAttorney.ToList();
                }
            }
            /// For user Staff Drop Down.....
            SResponse responseFOrStaff = Fetch.GotoService("api", $"UserManagement/GetStaff?ParentId={userId}", "GET");
            if (responseFOrStaff.Status && (responseFOrStaff.Resp != null) && (responseFOrStaff.Resp != ""))
            {
                List<AspNetUsers> Staff = JsonConvert.DeserializeObject<List<AspNetUsers>>(responseFOrStaff.Resp);
                ViewBag.GetStaff = new SelectList(Staff, "Id", "FirstName");
            }
            else
            {
                if (resp.Resp == "Please Add Firm Details First")
                {
                    List<AspNetUsers> Staff = new List<AspNetUsers>();
                    ViewBag.GetStaff = Staff.ToList();
                }
                else
                {
                    TempData["response"] = resp.Resp;
                    List<AspNetUsers> Staff = new List<AspNetUsers>();
                    ViewBag.GetStaff = Staff.ToList();
                }
            }
            SResponse responseForClient = Fetch.GotoService("api", $"UserManagement/GetStaff?ParentId={userId}&&userType=Client", "GET");
            if (responseForClient.Status && (responseForClient.Resp != null) && (responseForClient.Resp != ""))
            {
                List<AspNetUsers> Clients = JsonConvert.DeserializeObject<List<AspNetUsers>>(responseForClient.Resp);
                ViewBag.GetClient = Clients.Count;
            }
            else
            {
                if (resp.Resp == "Please Add Firm Details First")
                {
                    List<AspNetUsers> Clients = new List<AspNetUsers>();
                    ViewBag.GetClient = Clients.Count;
                }
                else
                {
                    TempData["response"] = resp.Resp;
                    List<AspNetUsers> Clients = new List<AspNetUsers>();
                    ViewBag.GetClient = Clients.Count;
                }
            }
            return View();
        }
        public IActionResult ManageCase()
        {
            string userId = string.Empty;
            string userdto = HttpContext.Session.GetString("userData");
            if (!string.IsNullOrEmpty(userdto))
            {
                UserDto userData = JsonConvert.DeserializeObject<UserDto>(userdto);
                userId = userData.Id;
            }
            SResponse resp = Fetch.GotoService("api", $"Firm/GetFirmByUserId?userId={userId}", "GET");
            if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
            {
                return View();
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
    }
}
