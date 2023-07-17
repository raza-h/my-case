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

namespace AbsolCase.Areas.Attorney.Controllers
{
    [ServiceFilter(typeof(ConfigureSession))]
    public class AttorneyDashboardController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AttorneyDashboardController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;


        }
        public IActionResult Index()
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
                SResponse resp = Fetch.GotoService("api", "News/GetAllNews", "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    List<News> ListOfNews = JsonConvert.DeserializeObject<List<News>>(resp.Resp);
                    ViewBag.News = ListOfNews.ToList().Where(x => x.SendTo == "SuperToAttorney").ToList();
                }
                else
                {
                    if (resp.Resp == "Please Add Firm Details First")
                    {
                        List<News> ListOfNews = new List<News>();
                        ViewBag.News = ListOfNews.ToList();
                    }
                    else
                    {
                        TempData["response"] = resp.Resp;
                        List<News> ListOfNews = new List<News>();
                        ViewBag.News = ListOfNews.ToList();
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
                        ViewBag.GetStaff = Staff.Count;
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
                SResponse responseForEvents = Fetch.GotoService("api", $"Events/GetEvents?userId={userId}", "GET");
                if (responseForEvents.Status && (responseForEvents.Resp != null) && (responseForEvents.Resp != ""))
                {
                    List<Events> eventsList = JsonConvert.DeserializeObject<List<Events>>(responseForEvents.Resp);
                    eventsList = eventsList.Where(x => x.Start.Value.Date >= DateTime.Now.Date).OrderBy(x => x.Start).ToList();
                    ViewBag.EventsList = eventsList;
                }
                else
                {
                    if (resp.Resp == "Please Add Firm Details First")
                    {
                        List<Events> eventsList = new List<Events>();
                        ViewBag.EventsList = eventsList.ToList();
                    }
                    else
                    {
                        TempData["response"] = resp.Resp;
                        List<Events> eventsList = new List<Events>();
                        ViewBag.EventsList = eventsList.ToList();
                    }
                }
                //SResponse responseForEvents = Fetch.GotoService("api", $"Events/GetEvents?userId={userId}", "GET");
                //if (responseForEvents.Status && (responseForEvents.Resp != null) && (responseForEvents.Resp != ""))
                //{
                //    List<Events> eventsList = JsonConvert.DeserializeObject<List<Events>>(responseForEvents.Resp);
                //    eventsList = eventsList.Where(x => x.Start.Value.Date >= DateTime.Now.Date).OrderBy(x => x.Start).Take(5).ToList();
                //    ViewBag.EventsList = eventsList;
                //}
                //else
                //{
                //    if (resp.Resp == "Please Add Firm Details First")
                //    {
                //        List<Events> eventsList = new List<Events>();
                //        ViewBag.EventsList = eventsList.ToList();
                //    }
                //    else
                //    {
                //        TempData["response"] = resp.Resp;
                //        List<Events> eventsList = new List<Events>();
                //        ViewBag.EventsList = eventsList.ToList();
                //    }
                //}

                SResponse responseForTasks = Fetch.GotoService("api", $"Task/GetTasks", "GET");
                if (responseForTasks.Status && (responseForTasks.Resp != null) && (responseForTasks.Resp != ""))
                {
                    List<Tasks> tasksList = JsonConvert.DeserializeObject<List<Tasks>>(responseForTasks.Resp);
                    tasksList = tasksList.Where(x => x.DueDate.Value.Date >= DateTime.Now.Date && x.Status == Models.TaskStatus.Active).ToList();
                    ViewBag.TasksList = tasksList;
                }
                else
                {
                    if (resp.Resp == "Please Add Firm Details First")
                    {
                        List<Events> eventsList = new List<Events>();
                        ViewBag.EventsList = eventsList.ToList();
                    }
                    else
                    {
                        TempData["response"] = resp.Resp;
                        List<Events> eventsList = new List<Events>();
                        ViewBag.EventsList = eventsList.ToList();
                    }
                }

                SResponse responseForRecentActivity = Fetch.GotoService("api", $"Activity/GetActivity", "GET");
                if (responseForRecentActivity.Status && (responseForRecentActivity.Resp != null) && (responseForRecentActivity.Resp != ""))
                {
                    List<AdminActivity> recentActivityList = JsonConvert.DeserializeObject<List<AdminActivity>>(responseForRecentActivity.Resp);
                    ViewBag.RecentActivity = recentActivityList;
                }
                else
                {
                    if (resp.Resp == "Please Add Firm Details First")
                    {
                        List<AdminActivity> recentActivityList = new List<AdminActivity>();
                        ViewBag.RecentActivity = recentActivityList.ToList();
                    }
                    else
                    {
                        TempData["response"] = resp.Resp;
                        List<AdminActivity> recentActivityList = new List<AdminActivity>();
                        ViewBag.RecentActivity = recentActivityList.ToList();
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
                    if (resp.Resp == "Please Add Firm Details First")
                    {
                        List<AspNetUsers> Cases = new List<AspNetUsers>();
                        ViewBag.GetCase = Cases.Count;
                    }
                    else
                    {
                        TempData["response"] = resp.Resp;
                        List<AspNetUsers> Cases = new List<AspNetUsers>();
                        ViewBag.GetCase = Cases.Count;
                    }
                }
                SResponse respRole = Fetch.GotoService("api", "UserManagement/GetRoles", "GET");
                if (respRole.Status && (respRole.Resp != null) && (respRole.Resp != ""))
                {
                    List<AspNetRoles> roles = JsonConvert.DeserializeObject<List<AspNetRoles>>(respRole.Resp).ToList();
                    List<AspNetRoles> Filterroles = roles.Where(x => x.Name != "Admin").ToList();
                    ViewBag.RolesList = Filterroles.Select(c => c.Name).ToList();
                }
                else
                {
                    if (resp.Resp == "Please Add Firm Details First")
                    {
                        List<AspNetRoles> roles = new List<AspNetRoles>();
                        ViewBag.RolesList = roles.ToList();
                    }
                    else
                    {
                        TempData["response"] = resp.Resp;
                        List<AspNetRoles> roles = new List<AspNetRoles>();
                        ViewBag.RolesList = roles.ToList();
                    }
                }
                return View();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public IActionResult GetAttorney(string Id)
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", $"UserManagement/GetUserById?Id={Id}", "GET");
                AspNetUsers user = JsonConvert.DeserializeObject<AspNetUsers>(resp.Resp);
                return Json(user);
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpGet]
        public IActionResult GetAllStaff(string Id)
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
                /// For user Staff Drop Down.....
                SResponse responseFOrStaff = Fetch.GotoService("api", $"UserManagement/GetStaff?ParentId={userId}", "GET");
                if (responseFOrStaff.Status && (responseFOrStaff.Resp != null) && (responseFOrStaff.Resp != ""))
                {
                    List<AspNetUsers> Staff = JsonConvert.DeserializeObject<List<AspNetUsers>>(responseFOrStaff.Resp);
                    var jsonResult = Json(Staff);
                    return jsonResult;
                    //return Json(Staff);
                }
                else
                {
                    TempData["response"] = responseFOrStaff.Resp;
                    List<AspNetUsers> Staff = new List<AspNetUsers>();
                    return Json(Staff);
                }
                
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public IActionResult GetAllAttorney(string Id)
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
                /// For user Staff Drop Down.....
                SResponse responseFOrUser = Fetch.GotoService("api", $"UserManagement/GetUsers?ParentId={userId}", "GET");
                if (responseFOrUser.Status && (responseFOrUser.Resp != null) && (responseFOrUser.Resp != ""))
                {
                    List<AspNetUsers> LeadAttorney = JsonConvert.DeserializeObject<List<AspNetUsers>>(responseFOrUser.Resp);
                    var jsonResult = Json(LeadAttorney);
                    return jsonResult;
                }
                else
                {
                    TempData["response"] = responseFOrUser.Resp;
                    List<AspNetUsers> LeadAttorney = new List<AspNetUsers>();
                    return Json(LeadAttorney);
                }

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
