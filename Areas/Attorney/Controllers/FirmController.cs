using AbsolCase.Configurations;
using AbsolCase.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using static AbsolCase.Configuration.RequestSender;

namespace AbsolCase.Areas.Attorney.Controllers
{
    [ServiceFilter(typeof(ConfigureSession))]
    public class FirmController : Controller
    {
        public IActionResult AddFirm()
        {
            try
            {
                Firm firm = new Firm();
                string userId = string.Empty;
                string userdto = HttpContext.Session.GetString("userData");
                if (!string.IsNullOrEmpty(userdto))
                {
                    UserDto userData = JsonConvert.DeserializeObject<UserDto>(userdto);
                    userId = userData.Id;
                    ViewBag.UserId = userId;
                }
                SResponse response = Fetch.GotoService("api", "Country/Get", "GET");
                if (response.Status && (response.Resp != null) && (response.Resp != ""))
                {
                    List<Country> countries = JsonConvert.DeserializeObject<List<Country>>(response.Resp).ToList();
                    ViewBag.Countries = countries;
                }
                SResponse res = Fetch.GotoService("api", "Country/GetStates", "GET");
                if (res.Status && (res.Resp != null) && (res.Resp != ""))
                {
                    List<State> states = JsonConvert.DeserializeObject<List<State>>(res.Resp).ToList();
                    ViewBag.States = states;
                }

                SResponse resp = Fetch.GotoService("api", $"Firm/GetFirmByUserId?userId={userId}", "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    firm = JsonConvert.DeserializeObject<Firm>(resp.Resp);
                    TempData["response"] = null;
                    return View(firm);
                }
                else
                {
                    TempData["response"] = "Please Add Firm Details First";
                    List<State> states = new List<State>();
                    ViewBag.States = states;
                    List<Country> countries = new List<Country>();
                    ViewBag.Countries = countries;
                }
                return View(firm);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public IActionResult ManageFirm()
        {
            return View();
        }

        public IActionResult LoadOfficeForm(int Count = 0)
        {
            return PartialView("~/Views/PartialViews/_OfficeForm.cshtml", Count);
        }
        public IActionResult LoadOfficeAddressForm(int office, int address)
        {
            ViewBag.Address = address;
            return PartialView("~/Views/PartialViews/_OfficeAddressForm.cshtml", office);
        }

        public IActionResult FirmUsers()
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", "UserManagement/GetFirmUsers", "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    List<AspNetUsers> _resultModel = JsonConvert.DeserializeObject<List<AspNetUsers>>(resp.Resp);
                    return View(_resultModel.Where(x=>x.RoleName!="Client").ToList());
                }
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
                return View();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public IActionResult AddUsers()
        {
            try
            {
                string userId = string.Empty;
                string userData = HttpContext.Session.GetString("userData");
                if (!string.IsNullOrEmpty(userData))
                {
                    UserDto userDto = JsonConvert.DeserializeObject<UserDto>(userData);
                    userId = userDto.Id;
                }
                SResponse responseFOrUser = Fetch.GotoService("api", $"UserManagement/GetUsers?ParentId={userId}", "GET");
                if (responseFOrUser.Status && (responseFOrUser.Resp != null) && (responseFOrUser.Resp != ""))
                {
                    List<AspNetUsers> LeadAttorney = JsonConvert.DeserializeObject<List<AspNetUsers>>(responseFOrUser.Resp);
                    ViewBag.LeadAttorney = new SelectList(LeadAttorney, "Id", "FirstName");
                }
                else
                {
                    if (responseFOrUser.Resp == "Please Add Firm Details First")
                    {
                        List<AspNetUsers> LeadAttorney = new List<AspNetUsers>();
                        ViewBag.LeadAttorney = null;
                    }
                    else
                    {
                        TempData["response"] = responseFOrUser.Resp;
                        List<AspNetUsers> LeadAttorney = new List<AspNetUsers>();
                        ViewBag.LeadAttorney = null;// LeadAttorney.ToList();
                    }
                }
                
                SResponse responseForCase = Fetch.GotoService("api", $"Case/GetCasesCreatedByTotal?CreatedBy={userId}", "GET");
                if (responseForCase.Status && (responseForCase.Resp != null) && (responseForCase.Resp != ""))
                {
                    List<AspNetUsers> Cases = JsonConvert.DeserializeObject<List<AspNetUsers>>(responseForCase.Resp);
                    ViewBag.GetCase = Cases.Count;
                }
                else
                {
                    if (responseFOrUser.Resp == "Please Add Firm Details First")
                    {
                        List<AspNetUsers> Cases = new List<AspNetUsers>();
                        ViewBag.GetCase = Cases.Count;
                    }
                    else
                    {
                        TempData["response"] = responseFOrUser.Resp;
                        List<AspNetUsers> Cases = new List<AspNetUsers>();
                        ViewBag.GetCase = Cases.Count;
                    }
                }
                SResponse resp = Fetch.GotoService("api", "UserManagement/GetFirmUsers", "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    List<AspNetUsers> _resultModel = JsonConvert.DeserializeObject<List<AspNetUsers>>(resp.Resp);
                    return View(_resultModel.Where(x=>x.RoleName!="Client").ToList());
                }
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
                return View();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public IActionResult AllSettings()
        {
            try
            {
                string userId = string.Empty;
                string userData = HttpContext.Session.GetString("userData");
                if (!string.IsNullOrEmpty(userData))
                {
                    UserDto userDto = JsonConvert.DeserializeObject<UserDto>(userData);
                    ViewBag.UserData = userDto;
                }
                SResponse resp = Fetch.GotoService("api", "UserManagement/GetFirmUsers", "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    List<AspNetUsers> _resultModel = JsonConvert.DeserializeObject<List<AspNetUsers>>(resp.Resp);
                    return View(_resultModel.Where(x => x.RoleName != "Client").ToList());
                }
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
                return View();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
