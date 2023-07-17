using AbsolCase.Configurations;
using Microsoft.AspNetCore.Mvc;
using AbsolCase.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static AbsolCase.Configuration.RequestSender;

namespace AbsolCase.Areas.Public.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ContactUs()
        {
            return View();
        }
            

       
        [HttpPost]
        public IActionResult AddContact(ContactUs model)
        {
            try
            {
                
                var body = JsonConvert.SerializeObject(model);
                SResponse resp = AbsolCase.Configuration.RequestSender.Fetch.GotoService("api", "ContactUs/AddContact", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ContactUs");
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
        [HttpGet]
        public IActionResult ManageContact()
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", "ContactUs/GetContact", "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    List<ContactUs> _resultModel = JsonConvert.DeserializeObject<List<ContactUs>>(resp.Resp);

                    return View(_resultModel);
                }
                else
                    return View();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet]

        public IActionResult UpdateContact(int Id)
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", $"ContactUs/GetContactById?Id={Id}", "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    ContactUs contactUs = JsonConvert.DeserializeObject<ContactUs>(resp.Resp);
                    return View(contactUs);
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
        public IActionResult UpdateContact(ContactUs model)
        {
            try
            {
                var body = JsonConvert.SerializeObject(model);
                SResponse resp = Fetch.GotoService("api", "ContactUs/UpdateContact", "POST", body);
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
            catch (Exception ex)
            {
                throw;
            }
        }
        public IActionResult DeleteContact(int Id)
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", $"ContactUs/DeleteContact?Id={Id}", "DELETE");
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




    }
}
