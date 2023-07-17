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

namespace AbsolCase.Areas.Administration.Controllers
{
    public class FAQAController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddFAQuestions()
        {
            Faq model = new Faq();

            model.CreatedDate = DateTime.UtcNow;

            return View(model);
        }
        [HttpPost]
        public IActionResult AddFAQuestions(Faq model)
        {
            try
            {
                model.CreatedDate = DateTime.Now;
               
                string userData = HttpContext.Session.GetString("userData");
                if (!string.IsNullOrEmpty(userData))
                {
                    UserDto userDto = JsonConvert.DeserializeObject<UserDto>(userData);
                    model.CreatedBy = userDto.Id;
                }
                var body = JsonConvert.SerializeObject(model);
                SResponse resp = AbsolCase.Configuration.RequestSender.Fetch.GotoService("api", "Faq/AddFaqs", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ManageFAQuestions");
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

        public IActionResult ManageFAQuestions()
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", "Faq/GetFaqs", "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    List<Faq> _resultModel = JsonConvert.DeserializeObject<List<Faq>>(resp.Resp);
                
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
        public IActionResult UpdateFAQuestions(int Id)
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", $"Faq/GetFaqsById?Id={Id}", "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    Faq _Faq = JsonConvert.DeserializeObject<Faq>(resp.Resp);
                    return View(_Faq);
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
        public IActionResult UpdateFAQuestions(Faq model)
        {
            try
            {
                var body = JsonConvert.SerializeObject(model);
                SResponse resp = Fetch.GotoService("api", "Faq/UpdateFaqs", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ManageFAQuestions");
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
        public IActionResult DeleteFAQuestions(int Id)
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", $"Faq/DeleteFaqs?Id={Id}", "DELETE");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ManageFAQuestions");
                }
                return RedirectToAction("ManageFAQuestions");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
