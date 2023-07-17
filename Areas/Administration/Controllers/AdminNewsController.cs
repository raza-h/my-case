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
    public class AdminNewsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddNews()
        {
            News model = new News();
            model.PublishDate = DateTime.Now;
            return View(model);
        }

        [HttpPost]
        public IActionResult AddNews(News model)
        {
            try
            {
                model.SendTo = "SuperToAttorney";
                model.FirmId = 0;
                var body = JsonConvert.SerializeObject(model);
                SResponse resp = Fetch.GotoService("api", "News/AddNewsAdmin", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ManageNews");
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

        public IActionResult UpdateNews(int Id)
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", $"News/GetNewsById?Id={Id}", "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    News model = JsonConvert.DeserializeObject<News>(resp.Resp);
                    return View(model);
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
        public IActionResult UpdateNews(News model)
        {
            try
            {
                var body = JsonConvert.SerializeObject(model);
                SResponse resp = Fetch.GotoService("api", "News/UpdateNews", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ManageNews");
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
        public IActionResult DeleteNews(int Id)
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", $"News/DeleteNews?Id={Id}", "DELETE");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ManageNews");
                }
                return RedirectToAction("ManageNews");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IActionResult ManageNews()
        {
            SResponse resp = Fetch.GotoService("api", "News/GetAllNews", "GET");
            if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
            {
                List<News> _newsList = JsonConvert.DeserializeObject<List<News>>(resp.Resp);
                List<News> NewsList = _newsList.ToList().Where(x => x.SendTo == "SuperToAttorney").ToList();
                return View(NewsList);
            }
            else
                return View();
        }
         
        public IActionResult DetailsNews(int Id)
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", $"News/GetNewsById?Id={Id}", "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    News model = JsonConvert.DeserializeObject<News>(resp.Resp);
                    return View(model);
                }
                else
                    return View();
            }
            catch (Exception ex)
            {
                throw;
            }
        }


    }
}
