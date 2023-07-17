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

namespace AbsolCase.Areas.Lead.Controllers
{
    [ServiceFilter(typeof(ConfigureSession))]
    public class LeadController : Controller
    {
        public IActionResult AddPotentialClient()
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
                TempData["response"] = "Please Add Firm Details First";
                return RedirectToAction("AddFirm", "Firm", new { area = "Attorney" });
            }
            return View();
        }
        public IActionResult UpdateLead(int Id)
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", $"Lead/GetLeadById?Id={Id}", "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    LeadDetail lead = JsonConvert.DeserializeObject<LeadDetail>(resp.Resp);
                    return View(lead);
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
        public IActionResult UpdateLead(LeadDetail lead)
        {
            try
            {
                var body = JsonConvert.SerializeObject(lead);
                SResponse resp = Fetch.GotoService("api", "Lead/UpdateLead", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ManageLead");
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
        public IActionResult ManageLead()
        {
            SResponse resplead = Fetch.GotoService("api", "Lead/GetLeads", "GET");
            if (resplead.Status && (resplead.Resp != null) && (resplead.Resp != ""))
            {
                List<LeadDetail> lead = JsonConvert.DeserializeObject<List<LeadDetail>>(resplead.Resp);
                return View(lead);
            }
            else
            {
                if (resplead.Resp == "Please Add Firm Details First")
                {
                    return RedirectToAction("AddFirm", "Firm", new { area = "Attorney" });
                }
            }
            return View();
        }
        public IActionResult BoardLead()
        {
            SResponse resplead = Fetch.GotoService("api", "Lead/GetLeads", "GET");
            if (resplead.Status && (resplead.Resp != null) && (resplead.Resp != ""))
            {
                List<LeadDetail> lead = JsonConvert.DeserializeObject<List<LeadDetail>>(resplead.Resp);
                return View(lead);
            }
            else
            {
                if (resplead.Resp == "Please Add Firm Details First")
                {
                    return RedirectToAction("AddFirm", "Firm", new { area = "Attorney" });
                }
            }
            return View();
        }
        public IActionResult DeleteLead(int Id)
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", $"Lead/DeleteLead?Id={Id}", "DELETE");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ManageLead");
                }
                return RedirectToAction("ManageLead");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
