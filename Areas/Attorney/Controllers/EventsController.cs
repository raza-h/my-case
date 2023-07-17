using AbsolCase.Configurations;
using AbsolCase.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using static AbsolCase.Configuration.RequestSender;

namespace AbsolCase.Areas.Attorney.Controllers
{
    [ServiceFilter(typeof(ConfigureSession))]
    public class EventsController : Controller
    {

        private readonly IHttpContextAccessor _httpContextAccessor;

        public EventsController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;


        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ManageEvents()
        {
            return View();
        }


        [HttpGet]
        public IActionResult GetCalendarEvents()
        {
            string userId = string.Empty;
            string userData = HttpContext.Session.GetString("userData");
            if (!string.IsNullOrEmpty(userData))
            {
                UserDto userDto = JsonConvert.DeserializeObject<UserDto>(userData);
                userId = userDto.Id;
            }
            SResponse resp = Fetch.GotoService("api", $"Events/GetEvents?userId={userId}", "GET");
            if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
            {
                List<Events> eventsList = JsonConvert.DeserializeObject<List<Events>>(resp.Resp);
                return Json(eventsList);
            }
            else
                return View();



        }

        [HttpPost]  
        public IActionResult AddUpdateEvents(Events model)
        {
            try
            {
                string UserId = string.Empty;
                string userData = HttpContext.Session.GetString("userData");
                if (!string.IsNullOrEmpty(userData))
                {
                    UserDto userDto = JsonConvert.DeserializeObject<UserDto>(userData);
                    model.UserId = userDto.Id;
                }
           

                var body = JsonConvert.SerializeObject(model);
      

             
                SResponse resp = Fetch.GotoService("api", "Events/AddUpdateEvents", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return Json("Added");
                }
                else
                {
                    TempData["response"] = resp.Resp + " " + "Unable To Add Events";
                    return RedirectToAction("ManageEvents");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IActionResult DeleteEvents(int Id)
        {
            try
            {

                SResponse resp = Fetch.GotoService("api", $"Events/DeleteEvents?Id={Id}", "DELETE");
                return Json("Ok");

            }
            catch (Exception ex)
            {
                RedirectToAction("ManageEvents");
                throw;
            }   
        }


    }
}
