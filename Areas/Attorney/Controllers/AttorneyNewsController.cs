using AbsolCase.Configurations;
using AbsolCase.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using static AbsolCase.Configuration.RequestSender;

namespace AbsolCase.Areas.Attorney.Controllers
{
    [ServiceFilter(typeof(ConfigureSession))]
    public class AttorneyNewsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddNews()
        {
            try
            {

                News model = new News();

                model.PublishDate = DateTime.UtcNow;



                return View(model);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpPost]
        public IActionResult AddNews(News model)
        {
            try
            {
                model.SendTo = "AttorneyToClient";
                var body = JsonConvert.SerializeObject(model);
                SResponse resp = Fetch.GotoService("api", "News/AddNews", "POST", body);
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
            try
            {
                SResponse resp = Fetch.GotoService("api", "News/GetAllNews", "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    List<News> _newsList = JsonConvert.DeserializeObject<List<News>>(resp.Resp);
                    List<News> NewsList = _newsList.ToList().Where(x => x.SendTo == "AttorneyToClient").ToList();
                    return View(NewsList);
                }
                else
                    return View();
            }
            catch (Exception ex)
            {

                throw; 
            }
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
