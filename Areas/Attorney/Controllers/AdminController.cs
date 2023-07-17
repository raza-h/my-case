using AbsolCase.Configurations;
using AbsolCase.Models;
using AspNetCoreHero.ToastNotification.Notyf.Models;
using DocuSign.Integrations.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph.ExternalConnectors;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using Rotativa.AspNetCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static AbsolCase.Configuration.RequestSender;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.WebUtilities;

namespace AbsolCase.Areas.Attorney.Controllers
{
    [ServiceFilter(typeof(ConfigureSession))]
    public class AdminController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration configuration;

        public AdminController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            this.configuration = configuration;


        }
        public IActionResult Index()
        {
            try
            {
                string userId = string.Empty;
                string ParentId = string.Empty;
                string userData = HttpContext.Session.GetString("userData");
                if (!string.IsNullOrEmpty(userData))
                {
                    UserDto userDto = JsonConvert.DeserializeObject<UserDto>(userData);
                    userId = userDto.Id;
                    ParentId = userDto.Id;
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
                List<CaseDetail> respp = new List<CaseDetail>();
                SResponse Cresp = Fetch.GotoService("api", $"Case/GetCasesCustomFields", "GET");
                if (Cresp.Status && (Cresp.Resp != null) && (Cresp.Resp != ""))
                {
                    respp = JsonConvert.DeserializeObject<List<CaseDetail>>(Cresp.Resp);
                }
                List<CustomField> resppTime = new List<CustomField>();
                SResponse CrespTime = Fetch.GotoService("api", $"AttorneyAdmin/GetTimeEntryFields", "GET");
                if (CrespTime.Status && (CrespTime.Resp != null) && (CrespTime.Resp != ""))
                {
                    resppTime = JsonConvert.DeserializeObject<List<CustomField>>(CrespTime.Resp);
                }
                //respp.ElementAt(0).customFieldTime = resppTime;
                string data = JsonConvert.SerializeObject(resppTime);
                HttpContext.Session.SetString("customFields", data);
                return View(respp);
            }
            catch (Exception)
            {
                throw;
            }

        }
        public IActionResult Settings()
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
        public IActionResult AllSettings()
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
                    return View(_resultModel.Where(x => x.RoleName != "Client").ToList());
                }
                if (resp.Resp == "Please Add Firm Details First")
                {
                    TempData["response"] = "Please Add Firm Details First";
                    return RedirectToAction("AddFirm", "Admin", new { area = "Attorney" });
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
            //try
            //{
            //    string userId = string.Empty;
            //    string userData = HttpContext.Session.GetString("userData");
            //    if (!string.IsNullOrEmpty(userData))
            //    {
            //        UserDto userDto = JsonConvert.DeserializeObject<UserDto>(userData);
            //        ViewBag.UserData = userDto;
            //    }
            //    SResponse resp = Fetch.GotoService("api", "UserManagement/GetFirmUsers", "GET");
            //    if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
            //    {
            //        List<AspNetUsers> _resultModel = JsonConvert.DeserializeObject<List<AspNetUsers>>(resp.Resp);
            //        return View(_resultModel.Where(x => x.RoleName != "Client").ToList());
            //    }
            //    if (resp.Resp == "Please Add Firm Details First")
            //    {
            //        TempData["response"] = "Please Add Firm Details First";
            //        return RedirectToAction("AddFirm", "Admin", new { area = "Attorney" });
            //    }
            //    else
            //    {
            //        TempData["response"] = resp.Resp;
            //        return View();
            //    }
            //    return View();
            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}
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
                    return View(_resultModel.Where(x => x.RoleName != "Client").ToList());
                }
                if (resp.Resp == "Please Add Firm Details First")
                {
                    TempData["response"] = "Please Add Firm Details First";
                    return RedirectToAction("AddFirm", "Admin", new { area = "Attorney" });
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

        #region Custom Fields
        public IActionResult AddCustomFields()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddCustomFields(CustomField model)
        {
            try
            {
                var body = JsonConvert.SerializeObject(model);
                SResponse resp = Fetch.GotoService("api", "AttorneyAdmin/AddCustomField", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return Json(resp.Resp);
                }
                else
                {
                    if (resp.Resp == "Please Add Firm Details First")
                    {
                        TempData["response"] = "Please Add Firm Details First";
                        return RedirectToAction("AddFirm", "Admin", new { area = "Attorney" });
                    }
                    else
                    {
                        TempData["response"] = resp.Resp;
                        return View();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IActionResult GetCaseFields()
        {
            SResponse respp = Fetch.GotoService("api", "AttorneyAdmin/GetPracticeAreas", "GET");
            if (respp.Status && (respp.Resp != null) && (respp.Resp != ""))
            {
                List<PracticeArea> practiceAreas = JsonConvert.DeserializeObject<List<PracticeArea>>(respp.Resp);
                ViewBag.PracticeArea = new SelectList(practiceAreas, "Id", "PracticeAreaName");
            }
            else
            {
                if (respp.Resp == "Please Add Firm Details First")
                {
                    List<PracticeArea> practiceAreas = new List<PracticeArea>();
                    ViewBag.PracticeArea = null;
                }
                else
                {
                    TempData["response"] = respp.Resp;
                    List<PracticeArea> practiceAreas = new List<PracticeArea>();
                    ViewBag.PracticeArea = null;
                }
            }
            SResponse taskresp = Fetch.GotoService("api", "AttorneyAdmin/GetCaseFields", "GET");
            if (taskresp.Status && (taskresp.Resp != null) && (taskresp.Resp != ""))
            {
                List<CustomField> data = JsonConvert.DeserializeObject<List<CustomField>>(taskresp.Resp);
                return View(data);
            }
            else
            {
                if (taskresp.Resp == "Please Add Firm Details First")
                {
                    return RedirectToAction("AddFirm", "Admin", new { area = "Attorney" });
                }
            }
            return View();
        }
        public IActionResult GetContactFields()
        {
            SResponse taskresp = Fetch.GotoService("api", "AttorneyAdmin/GetContactsFields", "GET");
            if (taskresp.Status && (taskresp.Resp != null) && (taskresp.Resp != ""))
            {
                List<CustomField> data = JsonConvert.DeserializeObject<List<CustomField>>(taskresp.Resp);
                return View(data);
            }
            else
            {
                if (taskresp.Resp == "Please Add Firm Details First")
                {
                    return RedirectToAction("AddFirm", "Admin", new { area = "Attorney" });
                }
            }
            return View();
        }
        public IActionResult GetCompanyFields()
        {
            SResponse taskresp = Fetch.GotoService("api", "AttorneyAdmin/GetCompaniesFields", "GET");
            if (taskresp.Status && (taskresp.Resp != null) && (taskresp.Resp != ""))
            {
                List<CustomField> data = JsonConvert.DeserializeObject<List<CustomField>>(taskresp.Resp);
                return View(data);
            }
            else
            {
                if (taskresp.Resp == "Please Add Firm Details First")
                {
                    return RedirectToAction("AddFirm", "Admin", new { area = "Attorney" });
                }
            }
            return View();
        }
        public IActionResult GetExpenseFields()
        {
            SResponse taskresp = Fetch.GotoService("api", "AttorneyAdmin/GetTimeEntryFields", "GET");
            if (taskresp.Status && (taskresp.Resp != null) && (taskresp.Resp != ""))
            {
                List<CustomField> data = JsonConvert.DeserializeObject<List<CustomField>>(taskresp.Resp);
                return View(data);
            }
            else
            {
                if (taskresp.Resp == "Please Add Firm Details First")
                {
                    return RedirectToAction("AddFirm", "Admin", new { area = "Attorney" });
                }
            }
            return View();
        }
        public IActionResult UpdateCustomField(int Id)
        {
            try
            {
                SResponse updateres = AbsolCase.Configuration.RequestSender.Fetch.GotoService("api", $"AttorneyAdmin/GetCustomFieldbyId?Id={Id}", "GET");
                if (updateres.Status && (updateres.Resp != null) && (updateres.Resp != ""))
                {

                    CustomField customField = JsonConvert.DeserializeObject<CustomField>(updateres.Resp);
                    return View(customField);
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
        public IActionResult UpdateCustomField(CustomField model)
        {
            try
            {
                var body = JsonConvert.SerializeObject(model);
                SResponse resp = AbsolCase.Configuration.RequestSender.Fetch.GotoService("api", "AttorneyAdmin/UpdateCustomField", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("GetCaseFields");
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
        public IActionResult UpdateCustomContactField(int Id)
        {
            try
            {
                SResponse updateres = AbsolCase.Configuration.RequestSender.Fetch.GotoService("api", $"AttorneyAdmin/GetCustomFieldbyId?Id={Id}", "GET");
                if (updateres.Status && (updateres.Resp != null) && (updateres.Resp != ""))
                {

                    CustomField customField = JsonConvert.DeserializeObject<CustomField>(updateres.Resp);
                    return View(customField);
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
        public IActionResult UpdateCustomContactField(CustomField model)
        {
            try
            {
                var body = JsonConvert.SerializeObject(model);
                SResponse resp = AbsolCase.Configuration.RequestSender.Fetch.GotoService("api", "AttorneyAdmin/UpdateCustomField", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("GetContactFields");
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
        public IActionResult UpdateCustomCompanyField(int Id)
        {
            try
            {
                SResponse updateres = AbsolCase.Configuration.RequestSender.Fetch.GotoService("api", $"AttorneyAdmin/GetCustomFieldbyId?Id={Id}", "GET");
                if (updateres.Status && (updateres.Resp != null) && (updateres.Resp != ""))
                {

                    CustomField customField = JsonConvert.DeserializeObject<CustomField>(updateres.Resp);
                    return View(customField);
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
        public IActionResult UpdateCustomCompanyField(CustomField model)
        {
            try
            {
                var body = JsonConvert.SerializeObject(model);
                SResponse resp = AbsolCase.Configuration.RequestSender.Fetch.GotoService("api", "AttorneyAdmin/UpdateCustomField", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("GetCompanyFields");
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
        public IActionResult UpdateCustomExpenseField(int Id)
        {
            try
            {
                SResponse updateres = AbsolCase.Configuration.RequestSender.Fetch.GotoService("api", $"AttorneyAdmin/GetCustomFieldbyId?Id={Id}", "GET");
                if (updateres.Status && (updateres.Resp != null) && (updateres.Resp != ""))
                {

                    CustomField customField = JsonConvert.DeserializeObject<CustomField>(updateres.Resp);
                    return View(customField);
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
        public IActionResult UpdateCustomExpenseField(CustomField model)
        {
            try
            {
                var body = JsonConvert.SerializeObject(model);
                SResponse resp = AbsolCase.Configuration.RequestSender.Fetch.GotoService("api", "AttorneyAdmin/UpdateCustomField", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("GetExpenseFields");
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

        #endregion
        public IActionResult ManageEvents()
        {
            return View();
        }


        public IActionResult Test()
        {
            return View();
        }
        //public IActionResult Testfield()
        //{
        //    return View();
        //}

        #region Task
        public IActionResult AddTask()
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
                    return RedirectToAction("AddFirm", "Admin", new { area = "Attorney" });
                }
                else
                {
                    TempData["response"] = resp.Resp;
                    return View();
                }
            }
            return View();
        }
        public IActionResult ManageTask()
        {
            SResponse taskresp = Fetch.GotoService("api", "Task/GetTasks", "GET");
            if (taskresp.Status && (taskresp.Resp != null) && (taskresp.Resp != ""))
            {
                List<Tasks> task = JsonConvert.DeserializeObject<List<Tasks>>(taskresp.Resp);
                return View(task);
            }
            else
            {
                if (taskresp.Resp == "Please Add Firm Details First")
                {
                    return RedirectToAction("AddFirm", "Admin", new { area = "Attorney" });
                }
            }
            return View();
        }
        public IActionResult UpdateTask(int Id)
        {
            try
            {
                SResponse updateres = AbsolCase.Configuration.RequestSender.Fetch.GotoService("api", $"Task/GetTaskById?Id={Id}", "GET");
                if (updateres.Status && (updateres.Resp != null) && (updateres.Resp != ""))
                {

                    Tasks task = JsonConvert.DeserializeObject<Tasks>(updateres.Resp);
                    return View(task);
                }
                else
                    return View();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public IActionResult DeleteTask(int Id)
        {
            try
            {
                SResponse respdel = AbsolCase.Configuration.RequestSender.Fetch.GotoService("api", $"Task/DeleteTask?Id={Id}", "DELETE");
                if (respdel.Status && (respdel.Resp != null) && (respdel.Resp != ""))
                {
                    return RedirectToAction("ManageTask");
                }
                return RedirectToAction("ManageTask");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region DocumentTag

        public IActionResult AddDocumentTag()
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
                    return RedirectToAction("AddFirm", "Admin", new { area = "Attorney" });
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
        public IActionResult AddDocumentTag(DocumentTag model)
        {
            try
            {
                var body = JsonConvert.SerializeObject(model);
                SResponse resp = Fetch.GotoService("api", "AttorneyAdmin/AddDocumentTag", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ManageDocumentTag");
                }
                else
                {
                    if (resp.Resp == "Please Add Firm Details First")
                    {
                        TempData["response"] = "Please Add Firm Details First";
                        return RedirectToAction("AddFirm", "Admin", new { area = "Attorney" });
                    }
                    else
                    {
                        TempData["response"] = resp.Resp;
                        return View();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IActionResult ManageDocumentTag()
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", "AttorneyAdmin/GetDocumentTag", "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    List<DocumentTag> _resultModel = JsonConvert.DeserializeObject<List<DocumentTag>>(resp.Resp);
                    return View(_resultModel);
                }
                if (resp.Resp == "Please Add Firm Details First")
                {
                    TempData["response"] = "Please Add Firm Details First";
                    return RedirectToAction("AddFirm", "Admin", new { area = "Attorney" });
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
        public IActionResult UpdateDocumentTag(int Id)
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", $"AttorneyAdmin/GetDocumentTagById?Id={Id}", "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    DocumentTag _documentTag = JsonConvert.DeserializeObject<DocumentTag>(resp.Resp);
                    return View(_documentTag);
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
        public IActionResult UpdateDocumentTag(DocumentTag model)
        {
            try
            {
                var body = JsonConvert.SerializeObject(model);
                SResponse resp = AbsolCase.Configuration.RequestSender.Fetch.GotoService("api", "AttorneyAdmin/UpdateDocumentTag", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ManageDocumentTag");
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
        public IActionResult DeleteDocumentTag(int Id)
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", $"AttorneyAdmin/DeleteDocumentTag?Id={Id}", "DELETE");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ManageDocumentTag");
                }
                return RedirectToAction("ManageDocumentTag");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Notes Tag
        public IActionResult AddNotesTag()
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
                    return RedirectToAction("AddFirm", "Admin", new { area = "Attorney" });
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
        public IActionResult AddNotesTag(NotesTag model)
        {
            try
            {
                var body = JsonConvert.SerializeObject(model);
                SResponse resp = Fetch.GotoService("api", "AttorneyAdmin/AddNotesTag", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ManageNotesTag");
                }
                else
                {
                    if (resp.Resp == "Please Add Firm Details First")
                    {
                        TempData["response"] = "Please Add Firm Details First";
                        return RedirectToAction("AddFirm", "Admin", new { area = "Attorney" });
                    }
                    else
                    {
                        TempData["response"] = resp.Resp;
                        return View();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IActionResult ManageNotesTag()
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", "AttorneyAdmin/GetNotesTag", "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    List<NotesTag> _resultModel = JsonConvert.DeserializeObject<List<NotesTag>>(resp.Resp);
                    return View(_resultModel);
                }
                if (resp.Resp == "Please Add Firm Details First")
                {
                    TempData["response"] = "Please Add Firm Details First";
                    return RedirectToAction("AddFirm", "Admin", new { area = "Attorney" });
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
        public IActionResult UpdateNotesTag(int Id)
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", $"AttorneyAdmin/GetNotesTagById?Id={Id}", "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    NotesTag _notesTag = JsonConvert.DeserializeObject<NotesTag>(resp.Resp);
                    return View(_notesTag);
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
        public IActionResult UpdateNotesTag(NotesTag model)
        {
            try
            {
                var body = JsonConvert.SerializeObject(model);
                SResponse resp = Fetch.GotoService("api", "AttorneyAdmin/UpdateNotesTag", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ManageNotesTag");
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
        public IActionResult DeleteNotesTag(int Id)
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", $"AttorneyAdmin/DeleteNotesTag?Id={Id}", "DELETE");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ManageNotesTag");
                }
                return RedirectToAction("ManageNotesTag");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region refferal source
        public IActionResult AddRefferalSource()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddRefferalSource(RefferalSource refferalSource)
        {
            try
            {
                var body = JsonConvert.SerializeObject(refferalSource);
                SResponse resp = Fetch.GotoService("api", "AttorneyAdmin/AddRefferalSource", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ManageRefferalSource");
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
        public IActionResult ManageRefferalSource()
        {
            try
            {
                SResponse resp = AbsolCase.Configuration.RequestSender.Fetch.GotoService("api", "AttorneyAdmin/GetRefferalSources", "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    List<RefferalSource> refferalSource = JsonConvert.DeserializeObject<List<RefferalSource>>(resp.Resp);
                    return View(refferalSource);
                }
                else
                    return View();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public IActionResult UpdateRefferalSource(int Id)
        {
            try
            {
                SResponse resp = AbsolCase.Configuration.RequestSender.Fetch.GotoService("api", $"AttorneyAdmin/GetRefferalSourceById?Id={Id}", "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    RefferalSource refferalSource = JsonConvert.DeserializeObject<RefferalSource>(resp.Resp);
                    return View(refferalSource);
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
        public IActionResult UpdateRefferalSource(RefferalSource refferalSource)
        {
            try
            {
                var body = JsonConvert.SerializeObject(refferalSource);
                SResponse resp = Fetch.GotoService("api", "AttorneyAdmin/UpdateRefferalSource", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ManageRefferalSource");
                }
                else
                {
                    TempData["response"] = resp.Resp + " " + "Unable To update Refferal Source";
                    return View();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public IActionResult DeleteRefferalSource(int Id)
        {
            try
            {
                SResponse resp = AbsolCase.Configuration.RequestSender.Fetch.GotoService("api", $"AttorneyAdmin/DeleteRefferalSource?Id={Id}", "DELETE");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ManageRefferalSource");
                }
                return RedirectToAction("ManageRefferalSource");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Billing Method
        public IActionResult AddBillingMethod()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddBillingMethod(BillingMethod billingMethod)
        {
            try
            {
                var body = JsonConvert.SerializeObject(billingMethod);
                SResponse resp = Fetch.GotoService("api", "AttorneyAdmin/AddBillingMethod", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ManageBillingMethod");
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
        public IActionResult ManageBillingMethod()
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", "AttorneyAdmin/GetBillingMethods", "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    List<BillingMethod> billingMethod = JsonConvert.DeserializeObject<List<BillingMethod>>(resp.Resp);
                    return View(billingMethod);
                }
                else
                    return View();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public IActionResult UpdateBillingMethod(int Id)
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", $"AttorneyAdmin/GetBillingMethodById?Id={Id}", "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    BillingMethod billingMethod = JsonConvert.DeserializeObject<BillingMethod>(resp.Resp);
                    return View(billingMethod);
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
        public IActionResult UpdateBillingMethod(BillingMethod billingMethod)
        {
            try
            {
                var body = JsonConvert.SerializeObject(billingMethod);
                SResponse resp = Fetch.GotoService("api", "AttorneyAdmin/UpdateBillingMethod", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ManageBillingMethod");
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
        public IActionResult DeleteBillingMethod(int Id)
        {
            try
            {
                SResponse resp = AbsolCase.Configuration.RequestSender.Fetch.GotoService("api", $"AttorneyAdmin/DeleteBillingMethod?Id={Id}", "DELETE");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ManageBillingMethod");
                }
                return RedirectToAction("ManageBillingMethod");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Contact Group
        public IActionResult AddContactGroup()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddContactGroup(ContactGroup contactGroup)
        {
            try
            {
                var body = JsonConvert.SerializeObject(contactGroup);
                SResponse resp = Fetch.GotoService("api", "AttorneyAdmin/AddContactGroup", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ManageContactGroup");
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
        public IActionResult ManageContactGroup()
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", "AttorneyAdmin/GetContactGroups", "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    List<ContactGroup> contactGroups = JsonConvert.DeserializeObject<List<ContactGroup>>(resp.Resp);
                    return View(contactGroups);
                }
                else
                    return View();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public IActionResult UpdateContactGroup(int Id)
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", $"AttorneyAdmin/GetContactGroupById?Id={Id}", "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    ContactGroup contactGroups = JsonConvert.DeserializeObject<ContactGroup>(resp.Resp);
                    return View(contactGroups);
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
        public IActionResult UpdateContactGroup(ContactGroup contactGroup)
        {
            try
            {
                var body = JsonConvert.SerializeObject(contactGroup);
                SResponse resp = Fetch.GotoService("api", "AttorneyAdmin/UpdateContactGroup", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ManageContactGroup");
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
        public IActionResult DeleteContactGroup(int Id)
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", $"AttorneyAdmin/DeleteContactGroup?Id={Id}", "DELETE");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ManageContactGroup");
                }
                return RedirectToAction("ManageContactGroup");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Practice Area
        public IActionResult AddPracticeArea()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddPracticeArea(PracticeArea contactGroup)
        {
            try
            {
                var body = JsonConvert.SerializeObject(contactGroup);
                SResponse resp = Fetch.GotoService("api", "AttorneyAdmin/AddPracticeArea", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ManagePracticeArea");
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
        public IActionResult ManagePracticeArea()
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", "AttorneyAdmin/GetPracticeAreas", "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    List<PracticeArea> practiceArea = JsonConvert.DeserializeObject<List<PracticeArea>>(resp.Resp);
                    return View(practiceArea);
                }
                else
                    return View();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public IActionResult UpdatePracticeArea(int Id)
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", $"AttorneyAdmin/GetPracticeAreaById?Id={Id}", "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    PracticeArea practiceArea = JsonConvert.DeserializeObject<PracticeArea>(resp.Resp);
                    return View(practiceArea);
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
        public IActionResult UpdatePracticeArea(PracticeArea practiceArea)
        {
            try
            {
                var body = JsonConvert.SerializeObject(practiceArea);
                SResponse resp = Fetch.GotoService("api", "AttorneyAdmin/UpdatePracticeArea", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ManagePracticeArea");
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
        public IActionResult DeletePracticeArea(int Id)
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", $"AttorneyAdmin/DeletePracticeArea?Id={Id}", "DELETE");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ManagePracticeArea");
                }
                return RedirectToAction("ManagePracticeArea");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Documents
        public IActionResult ManageDocuments()
        {
            try
            {
                string userId = string.Empty;
                string userdto = HttpContext.Session.GetString("userData");
                UserDto userData = JsonConvert.DeserializeObject<UserDto>(userdto);
                if (userData != null)
                    userId = userData.Id;
                SResponse resp = Fetch.GotoService("api", $"Documents/GetDocuments?UserId={userId}", "GET");
                if (resp.ErrorCode == 401)
                    return RedirectToAction("Login", "Account", new { area = "Security" });

                if (resp.Status && !string.IsNullOrEmpty(resp.Resp))
                {
                    List<Decuments> _resultModel = JsonConvert.DeserializeObject<List<Decuments>>(resp.Resp);
                    return View(_resultModel);
                }
                if (resp.Resp == "Please Add Firm Details First")
                {
                    TempData["response"] = "Please Add Firm Details First";
                    return RedirectToAction("AddFirm", "Admin", new { area = "Attorney" });
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
        public IActionResult ClientDocuments()
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", $"Documents/GetMyClientsDocuments", "GET");
                if (resp.ErrorCode == 401)
                    return RedirectToAction("Login", "Account", new { area = "Security" });

                if (resp.Status && !string.IsNullOrEmpty(resp.Resp))
                {
                    List<Decuments> _resultModel = JsonConvert.DeserializeObject<List<Decuments>>(resp.Resp);
                    return View(_resultModel);
                }
                if (resp.Resp == "Please Add Firm Details First")
                {
                    TempData["response"] = "Please Add Firm Details First";
                    return RedirectToAction("AddFirm", "Admin", new { area = "Attorney" });
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
        public IActionResult DownloadFiles(string id)
        {
            try
            {
                string fileExtention = "";
                string path = "";
                int docid = Convert.ToInt32(id);
                string _decumentPath = string.Empty;
                SResponse resp = AbsolCase.Configuration.RequestSender.Fetch.GotoService("api", $"Documents/GetAdminDocumentsById?Id={id}", "GET");
                Decuments DecumentResp = JsonConvert.DeserializeObject<Decuments>(resp.Resp);
                fileExtention = DecumentResp.DecumentPath.Substring(DecumentResp.DecumentPath.IndexOf('.') + 1);
                path = $"{configuration.GetValue<string>("App:RemoteServerUrl")}" + $"{DecumentResp.DecumentPath}";
                WebClient wc = new WebClient();
                //string path = fileName;
                //Read the File data into Byte Array.
                byte[] bytes = wc.DownloadData(path);
                string Filename = DecumentResp.DecumentTittle + "." + fileExtention;

                return File(bytes, "application/x-msdownload", Filename);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region Leads
        public IActionResult AddPotentialClient()
        {
            string userId = string.Empty;
            string userdto = HttpContext.Session.GetString("userData");
            if (!string.IsNullOrEmpty(userdto))
            {
                UserDto userData = JsonConvert.DeserializeObject<UserDto>(userdto);
                userId = userData.Id;
            }

            SResponse respp = Fetch.GotoService("api", "AttorneyAdmin/GetLeadStatus", "GET");
            if (respp.Status && (respp.Resp != null) && (respp.Resp != ""))
            {
                List<LeadStatus> leadStatuses = JsonConvert.DeserializeObject<List<LeadStatus>>(respp.Resp);
                ViewBag.LeadStatus = new SelectList(leadStatuses, "LStatusId", "LStatusName");
            }
            else
            {
                if (respp.Resp == "Please Add Firm Details First")
                {
                    List<LeadStatus> leadStatuses = new List<LeadStatus>();
                    ViewBag.LeadStatus = null;
                }
                else
                {
                    TempData["response"] = respp.Resp;
                    List<LeadStatus> leadStatuses = new List<LeadStatus>();
                    ViewBag.LeadStatus = null;
                }
            }


            SResponse resp = Fetch.GotoService("api", $"Firm/GetFirmByUserId?userId={userId}", "GET");
            if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
            {
                return View();
            }
            else
            {
                TempData["response"] = "Please Add Firm Details First";
                return RedirectToAction("AddFirm", "Admin", new { area = "Attorney" });
            }
            return View();
        }
        public IActionResult LeadDetails(int Id)
        {
            try
            {
                SResponse respp = Fetch.GotoService("api", "AttorneyAdmin/GetReasons", "GET");
                if (respp.Status && (respp.Resp != null) && (respp.Resp != ""))
                {
                    List<HireReason> hireReasons = JsonConvert.DeserializeObject<List<HireReason>>(respp.Resp);
                    ViewBag.HireReasons = new SelectList(hireReasons, "ReasonId", "ReasonName");
                }
                else
                {
                    if (respp.Resp == "Please Add Firm Details First")
                    {
                        List<HireReason> hireReasons = new List<HireReason>();
                        ViewBag.HireReasons = null;
                    }
                    else
                    {
                        TempData["response"] = respp.Resp;
                        List<HireReason> hireReasons = new List<HireReason>();
                        ViewBag.HireReasons = null;
                    }
                }

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
        public IActionResult UpdateLead(int Id)
        {
            try
            {
                SResponse respp = Fetch.GotoService("api", "AttorneyAdmin/GetLeadStatus", "GET");
                if (respp.Status && (respp.Resp != null) && (respp.Resp != ""))
                {
                    List<LeadStatus> leadStatuses = JsonConvert.DeserializeObject<List<LeadStatus>>(respp.Resp);
                    ViewBag.LeadStatus = new SelectList(leadStatuses, "LStatusId", "LStatusName");
                }
                else
                {
                    if (respp.Resp == "Please Add Firm Details First")
                    {
                        List<LeadStatus> leadStatuses = new List<LeadStatus>();
                        ViewBag.LeadStatus = null;
                    }
                    else
                    {
                        TempData["response"] = respp.Resp;
                        List<LeadStatus> leadStatuses = new List<LeadStatus>();
                        ViewBag.LeadStatus = null;
                    }
                }
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
                    return RedirectToAction("AddFirm", "Admin", new { area = "Attorney" });
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
                    return RedirectToAction("AddFirm", "Admin", new { area = "Attorney" });
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
        #endregion

        #region company & contact
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
                //return View();
                SResponse respp = Fetch.GotoService("api", "AttorneyAdmin/GetCompaniesFields", "GET");
                if (respp.Status && (respp.Resp != null) && (respp.Resp != ""))
                {
                    List<CustomField> customFields = JsonConvert.DeserializeObject<List<CustomField>>(respp.Resp);

                    Company company = new Company();
                    company.customField = customFields;
                    return View(company);
                }
            }
            else
            {
                if (resp.Resp == "Please Add Firm Details First")
                {
                    TempData["response"] = "Please Add Firm Details First";
                    return RedirectToAction("AddFirm", "Admin", new { area = "Attorney" });
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
                if (resp.Resp == "Please Add Firm Details First")
                {
                    return RedirectToAction("AddFirm", "Admin", new { area = "Attorney" });
                }
            }
            return View();
        }
        public IActionResult ViewCompany(int Id)
        {
            SResponse resp = Fetch.GotoService("api", $"CompanyManagement/GetById?Id={Id}", "GET");
            if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
            {
                Company company = JsonConvert.DeserializeObject<Company>(resp.Resp);
                return View(company);
            }
            else
            {
                if (resp.Resp == "Please Add Firm Details First")
                {
                    TempData["response"] = "Please Add Firm Details First";
                    return RedirectToAction("AddFirm", "Admin", new { area = "Attorney" });
                }
                else
                {
                    TempData["response"] = resp.Resp;
                    return View();
                }
            }
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
            var chkbit = false;
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
                //return View();
                SResponse respp = Fetch.GotoService("api", "AttorneyAdmin/GetContactsFields", "GET");
                if (respp.Status && (respp.Resp != null) && (respp.Resp != ""))
                {
                    List<CustomField> customFields = JsonConvert.DeserializeObject<List<CustomField>>(respp.Resp);

                    Contact contact = new Contact();
                    contact.customField = customFields;
                    return View(contact);
                }
            }
            else
            {
                if (resp.Resp == "Please Add Firm Details First")
                {
                    TempData["response"] = "Please Add Firm Details First";
                    return RedirectToAction("AddFirm", "Admin", new { area = "Attorney" });
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
                    return RedirectToAction("AddFirm", "Admin", new { area = "Attorney" });
                }
                else
                {
                    TempData["response"] = resp.Resp;
                    return View();
                }
            }
            return View();
        }
        public IActionResult ViewContact(int Id)
        {
            SResponse resp = Fetch.GotoService("api", $"Contact/GetById?Id={Id}", "GET");
            if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
            {
                Contact contact = JsonConvert.DeserializeObject<Contact>(resp.Resp);
                return View(contact);
            }
            else
            {
                if (resp.Resp == "Please Add Firm Details First")
                {
                    TempData["response"] = "Please Add Firm Details First";
                    return RedirectToAction("AddFirm", "Admin", new { area = "Attorney" });
                }
                else
                {
                    TempData["response"] = resp.Resp;
                    return View();
                }
            }
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
        #endregion

        #region Case
        public IActionResult ManageCases()
        {
            try
            {
                string userId = string.Empty;
                string ParentId = string.Empty;
                string userData = HttpContext.Session.GetString("userData");
                if (!string.IsNullOrEmpty(userData))
                {
                    UserDto userDto = JsonConvert.DeserializeObject<UserDto>(userData);
                    userId = userDto.Id;
                    ParentId = userDto.Id;
                }
                List<CaseDetail> respp = new List<CaseDetail>();
                SResponse resp = Fetch.GotoService("api", $"Case/GetCasesCreatedBy?CreatedBy={ParentId}", "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    respp = JsonConvert.DeserializeObject<List<CaseDetail>>(resp.Resp);
                }
                else
                {
                    if (resp.Resp == "Please Add Firm Details First")
                    {
                        TempData["response"] = "Please Add Firm Details First";
                        return RedirectToAction("AddFirm", "Admin", new { area = "Attorney" });
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
                return View(respp);
            }
            catch (Exception)
            {
                throw;
            }


        }

        public IActionResult UpdateCase(int Id)
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", $"Case/GetCasesById?Id={Id}", "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    CaseDetail _notesTag = JsonConvert.DeserializeObject<CaseDetail>(resp.Resp);
                    return View(_notesTag);
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
        public IActionResult UpdateCase(CaseDetail model)
        {
            try
            {
                var body = JsonConvert.SerializeObject(model);
                SResponse resp = Fetch.GotoService("api", "ConfigManagement/UpdateNotesTag", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ManageNotesTag");
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
        public IActionResult ViewCase(int Id)
        {
            try
            {

                string userId = string.Empty;
                string ParentId = string.Empty;
                string userData = HttpContext.Session.GetString("userData");
                if (!string.IsNullOrEmpty(userData))
                {
                    UserDto userDto = JsonConvert.DeserializeObject<UserDto>(userData);
                    userId = userDto.Id;
                    ParentId = userDto.Id;
                }

                SResponse responseForEvents = Fetch.GotoService("api", $"Events/GetEvents?userId={ParentId}", "GET");
                if (responseForEvents.Status && (responseForEvents.Resp != null) && (responseForEvents.Resp != ""))
                {
                    List<Events> eventsList = JsonConvert.DeserializeObject<List<Events>>(responseForEvents.Resp);
                    eventsList = eventsList.Where(x => x.Start.Value.Date >= DateTime.Now.Date).OrderBy(x => x.Start).ToList();
                    ViewBag.EventsList = eventsList;
                }
                else
                {
                    if (responseForEvents.Resp == "Please Add Firm Details First")
                    {
                        List<Events> eventsList = new List<Events>();
                        ViewBag.EventsList = eventsList.ToList();
                    }
                    else
                    {
                        TempData["response"] = responseForEvents.Resp;
                        List<Events> eventsList = new List<Events>();
                        ViewBag.EventsList = eventsList.ToList();
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
                    if (responseForEvents.Resp == "Please Add Firm Details First")
                    {
                        List<AspNetUsers> LeadAttorney = new List<AspNetUsers>();
                        ViewBag.LeadAttorney = null;
                    }
                    else
                    {
                        TempData["response"] = responseForEvents.Resp;
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
                    if (responseForEvents.Resp == "Please Add Firm Details First")
                    {
                        List<AspNetUsers> Staff = new List<AspNetUsers>();
                        ViewBag.GetStaff = Staff.ToList();
                    }
                    else
                    {
                        TempData["response"] = responseForEvents.Resp;
                        List<AspNetUsers> Staff = new List<AspNetUsers>();
                        ViewBag.GetStaff = Staff.Count;
                    }
                }

                SResponse responseForTasks = Fetch.GotoService("api", $"Task/GetTasks", "GET");
                if (responseForTasks.Status && (responseForTasks.Resp != null) && (responseForTasks.Resp != ""))
                {
                    List<Tasks> tasksList = JsonConvert.DeserializeObject<List<Tasks>>(responseForTasks.Resp);
                    tasksList = tasksList.Where(x => x.DueDate.Value.Date >= DateTime.Now.Date && x.Status == Models.TaskStatus.Active).ToList();
                    ViewBag.TasksList = tasksList;
                }
                else
                {
                    if (responseForEvents.Resp == "Please Add Firm Details First")
                    {
                        List<Events> eventsList = new List<Events>();
                        ViewBag.EventsList = eventsList.ToList();
                    }
                    else
                    {
                        TempData["response"] = responseForEvents.Resp;
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
                    if (responseForEvents.Resp == "Please Add Firm Details First")
                    {
                        List<AdminActivity> recentActivityList = new List<AdminActivity>();
                        ViewBag.RecentActivity = recentActivityList.ToList();
                    }
                    else
                    {
                        TempData["response"] = responseForEvents.Resp;
                        List<AdminActivity> recentActivityList = new List<AdminActivity>();
                        ViewBag.RecentActivity = recentActivityList.ToList();
                    }
                }
                
                SResponse resp = Fetch.GotoService("api", $"Case/GetCasesById?Id={Id}", "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    CaseDetail caseDetail = JsonConvert.DeserializeObject<CaseDetail>(resp.Resp);
                    return View(caseDetail);
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
        public IActionResult CloseCase(int id)
        {
            try
            {
                var body = JsonConvert.SerializeObject(id);
                SResponse resp = Fetch.GotoService("api", "Case/CloseCase", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ManageCases");
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
        public IActionResult CaseStages()
        {
            return View();
        }
        #endregion

        #region Message
        public IActionResult Message()
        {
            try
            {
                string userId = string.Empty;
                string userdto = HttpContext.Session.GetString("userData");
                UserDto userData = JsonConvert.DeserializeObject<UserDto>(userdto);
                if (userData != null)
                    userId = userData.Id;

                SResponse resp = Fetch.GotoService("api", $"Message/GetMessagesByUserId?userId={userId}", "GET");
                if (resp.ErrorCode == 401)
                    return RedirectToAction("Login", "Account", new { area = "Security" });

                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    List<Message> messages = JsonConvert.DeserializeObject<List<Message>>(resp.Resp);
                    if (messages != null && messages.Count > 0)
                    {
                        foreach (var message in messages)
                        {
                            if (!string.IsNullOrEmpty(message.UserImagePath) && !string.IsNullOrEmpty(message.UserImagePath.Trim()))
                                message.UserImagePath = $"{configuration.GetValue<string>("App:RemoteServerUrl")}{message.UserImagePath}";
                            else
                                message.UserImagePath = $"{configuration.GetValue<string>("App:RemoteServerUrl")}Images/blank-profile.png";
                        }
                    }
                    return View(messages);
                }
                else
                    return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IActionResult LoadChatMessages(string senderId, string receiverId)
        {
            try
            {
                string userId = string.Empty;
                string userdto = HttpContext.Session.GetString("userData");
                UserDto userData = JsonConvert.DeserializeObject<UserDto>(userdto);
                if (userData != null)
                    userId = userData.Id;

                SResponse resp = Fetch.GotoService("api", $"Message/GetMessagesBySenderIdAndReceiverId?senderId={senderId}&&receiverId={receiverId}&&userId={userId}", "GET");
                if (resp.ErrorCode == 401)
                    return RedirectToAction("Login", "Account", new { area = "Security" });

                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    List<Message> messages = JsonConvert.DeserializeObject<List<Message>>(resp.Resp);
                    if (messages != null && messages.Count > 0)
                    {
                        foreach (var message in messages)
                        {
                            if (!string.IsNullOrEmpty(message.ImagePath) && !string.IsNullOrEmpty(message.ImagePath.Trim()))
                                message.ImagePath = $"{configuration.GetValue<string>("App:RemoteServerUrl")}{message.ImagePath}";
                            if (!string.IsNullOrEmpty(message.UserImagePath))
                                message.UserImagePath = $"{configuration.GetValue<string>("App:RemoteServerUrl")}{message.UserImagePath}";
                            else
                                message.UserImagePath = $"{configuration.GetValue<string>("App:RemoteServerUrl")}Images/blank-profile.png";
                        }
                    }
                    return PartialView("~/Views/PartialViews/_Chat.cshtml", messages);
                }
                else
                    return Ok();
            }
            catch (Exception ex)
            {
                return Ok();
            }
        }

        public IActionResult LoadGroupMessages(int groupId)
        {
            try
            {
                string userId = string.Empty;
                string userdto = HttpContext.Session.GetString("userData");
                UserDto userData = JsonConvert.DeserializeObject<UserDto>(userdto);
                if (userData != null)
                    userId = userData.Id;

                SResponse resp = Fetch.GotoService("api", $"Message/GetGroupMessages?groupId={groupId}&&userId={userId}", "GET");
                if (resp.ErrorCode == 401)
                    return RedirectToAction("Login", "Account", new { area = "Security" });

                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    List<Message> messages = JsonConvert.DeserializeObject<List<Message>>(resp.Resp);
                    if (messages != null && messages.Count > 0)
                    {
                        foreach (var message in messages)
                        {
                            if (!string.IsNullOrEmpty(message.ImagePath) && !string.IsNullOrEmpty(message.ImagePath.Trim()))
                                message.ImagePath = $"{configuration.GetValue<string>("App:RemoteServerUrl")}{message.ImagePath}";
                            if (!string.IsNullOrEmpty(message.UserImagePath) && !string.IsNullOrEmpty(message.UserImagePath.Trim()))
                                message.UserImagePath = $"{configuration.GetValue<string>("App:RemoteServerUrl")}{message.UserImagePath}";
                            else
                                message.UserImagePath = $"{configuration.GetValue<string>("App:RemoteServerUrl")}Images/blank-profile.png";
                        }
                    }
                    return PartialView("~/Views/PartialViews/_Chat.cshtml", messages);
                }
                else
                    return Ok();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Expense
        public IActionResult AddExpense()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddExpense(Expense expense)
        {
            try
            {
                if (expense.Image != null && expense.Image.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        expense.Image.CopyTo(ms);
                        expense.File = ms.ToArray();
                        expense.Image = null;
                    }
                }
                var body = JsonConvert.SerializeObject(expense);
                SResponse resp = Fetch.GotoService("api", "Expense/AddExpense", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ManageExpense");
                }
                else
                {
                    if (resp.Resp == "Please Add Firm Details First")
                    {
                        TempData["response"] = "Please Add Firm Details First";
                        return RedirectToAction("AddFirm", "Admin", new { area = "Attorney" });
                    }
                    else
                    {
                        TempData["response"] = resp.Resp;
                        return View();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IActionResult ManageExpense()
        {
            SResponse resp = Fetch.GotoService("api", $"Expense/GetExpenses", "GET");
            if (resp.ErrorCode == 401)
                return RedirectToAction("Login", "Account", new { area = "Security" });

            if (resp.Status && !string.IsNullOrEmpty(resp.Resp))
            {
                List<ExpenseViewModel> _resultModel = JsonConvert.DeserializeObject<List<ExpenseViewModel>>(resp.Resp);
                return View(_resultModel);
            }
            else
            {
                if (resp.Resp == "Please Add Firm Details First")
                {
                    TempData["response"] = "Please Add Firm Details First";
                    return RedirectToAction("AddFirm", "Admin", new { area = "Attorney" });
                }
                else
                {
                    TempData["response"] = resp.Resp;
                    return View();
                }
            }
            return View();
        }

        public IActionResult DownloadExpense(string StaffName, string Amount, string Description, string CreatedDate, int id)
        {
            try
            {
                ExpenseViewModel model = new ExpenseViewModel();
                model.ClientName = StaffName;
                model.CreatedDate = CreatedDate;
                model.Id = id;
                model.Amount = Convert.ToInt32(Amount);
                model.Description = Description;
                var pdfResult = new ViewAsPdf(model);
                return pdfResult;

            }
            catch (Exception ex)
            {
                throw;
            }


        }
        #endregion

        #region Invoices
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
                    return RedirectToAction("AddFirm", "Admin", new { area = "Attorney" });
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
                    return RedirectToAction("AddFirm", "Admin", new { area = "Attorney" });
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
                    return RedirectToAction("AddFirm", "Admin", new { area = "Attorney" });
                }
                else
                {
                    TempData["response"] = resp.Resp;
                    return View();
                }
            }
            return View();
        }
        #endregion

        #region Notes
        public IActionResult ManageNotes()
        {
            try
            {
                string userId = string.Empty;
                string userdto = HttpContext.Session.GetString("userData");
                UserDto userData = JsonConvert.DeserializeObject<UserDto>(userdto);
                if (userData != null)
                    userId = userData.Id;
                SResponse resp = Fetch.GotoService("api", $"Notes/GetNotes?UserId={userId}", "GET");

                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    List<Notes> task = JsonConvert.DeserializeObject<List<Notes>>(resp.Resp);
                    return View(task);
                }
                else
                {
                    if (resp.Resp == "Please Add Firm Details First")
                    {
                        TempData["response"] = "Please Add Firm Details First";
                        return RedirectToAction("AddFirm", "Admin", new { area = "Attorney" });
                    }
                    else
                    {
                        TempData["response"] = resp.Resp;
                        return View();
                    }
                }
                return View();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public IActionResult DownloadNotes(string NotesTag, string NotesTittle, String NotesDescrpation)
        {
            try
            {
                Notes model = new Notes();
                model.NotesTag = NotesTag;
                model.NotesTittle = NotesTittle;
                model.NotesDescripation = NotesDescrpation;
                var pdfResult = new ViewAsPdf(model);
                return pdfResult;

            }
            catch (Exception ex)
            {
                throw;
            }


        }
        #endregion

        #region Profile
        public IActionResult AddUser()
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
                SResponse resps = Fetch.GotoService("api", "UserManagement/GetRoles", "GET");
                if (resps.Status && (resps.Resp != null) && (resps.Resp != ""))
                {
                    List<AspNetRoles> roles = JsonConvert.DeserializeObject<List<AspNetRoles>>(resps.Resp).ToList();
                    roles = roles.Where(x => x.Name != "Admin" && x.Name != "Client" && x.Name != "Customer").ToList();
                    ViewBag.RolesList = roles.Select(c => c.Name).ToList();
                }
            }
            else
            {
                if (resp.Resp == "Please Add Firm Details First")
                {
                    TempData["response"] = "Please Add Firm Details First";
                    return RedirectToAction("AddFirm", "Admin", new { area = "Attorney" });
                }
                else
                {
                    List<AspNetRoles> roles = new List<AspNetRoles>();
                    ViewBag.RolesList = roles.ToList();
                    TempData["response"] = resp.Resp;
                    return View();
                }
            }
            return View();
        }
        [HttpPost]
        public IActionResult AddUser(UserSignupDto user)
        {
            try
            {
                var body = JsonConvert.SerializeObject(user);
                SResponse resp = Fetch.GotoService("api", "UserManagement/InviteMember", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ManageUser");
                }
                else
                {
                    TempData["response"] = resp.Resp;
                    SResponse response = Fetch.GotoService("api", "UserManagement/GetRoles", "GET");
                    if (response.Status && (response.Resp != null) && (response.Resp != ""))
                    {
                        List<AspNetRoles> roles = JsonConvert.DeserializeObject<List<AspNetRoles>>(response.Resp).ToList();
                        roles = roles.Where(x => x.Name != "Admin" && x.Name != "Client" && x.Name != "Customer").ToList();
                        ViewBag.RolesList = roles.Select(c => c.Name).ToList();
                    }
                    return View();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IActionResult UserProfile()
        {
            string userdto = HttpContext.Session.GetString("userData");
            User model = new User();
            UserDto userDto = new UserDto();
            if (!string.IsNullOrEmpty(userdto))
            {
                userDto = JsonConvert.DeserializeObject<UserDto>(userdto);
                SResponse resp = Fetch.GotoService("api", $"UserManagement/GetUserById?Id={userDto.Id}", "GET");
                if (resp.Status && (!string.IsNullOrEmpty(resp.Resp)))
                {
                    model = JsonConvert.DeserializeObject<User>(resp.Resp);
                    if (!string.IsNullOrEmpty(model.ImagePath) && !string.IsNullOrEmpty(model.ImagePath.Trim()))
                    {
                        ViewBag.profileImage = model.ImagePath;
                        model.ImagePath = $"{configuration.GetValue<string>("App:RemoteServerUrl")}{model.ImagePath}";
                    }
                    else
                    {
                        model.ImagePath = $"{configuration.GetValue<string>("App:RemoteServerUrl")}Images/blank-profile.png";
                    }
                }
            }
            return View(model);
        }
        [HttpPost]
        public IActionResult UserProfile(User model)
        {
            if (model.Image != null && model.Image.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    model.Image.CopyTo(ms);
                    model.File = ms.ToArray();
                    model.Image = null;
                }
            }
            var body = JsonConvert.SerializeObject(model);
            SResponse resp = Fetch.GotoService("api", $"UserManagement/UpdateUser", "POST", body);
            if (resp.Status && (!string.IsNullOrEmpty(resp.Resp)))
            {
                string token = string.Empty;
                string roleName = string.Empty;
                UserDto sessionData = new UserDto();
                string sessionString = HttpContext.Session.GetString("userData");
                if (!string.IsNullOrEmpty(sessionString))
                {
                    sessionData = JsonConvert.DeserializeObject<UserDto>(sessionString);
                    token = sessionData.Token;
                    roleName = sessionData.RoleName;
                }
                HttpContext.Session.Remove("userData");
                UserDto userDto = JsonConvert.DeserializeObject<UserDto>(resp.Resp);
                if (!string.IsNullOrEmpty(userDto.ImagePath) && !string.IsNullOrEmpty(userDto.ImagePath.Trim()))
                    userDto.ImagePath = $"{configuration.GetValue<string>("App:RemoteServerUrl")}{userDto.ImagePath}";
                else
                    userDto.ImagePath = $"{configuration.GetValue<string>("App:RemoteServerUrl")}Images/blank-profile.png";

                userDto.Token = token;
                userDto.RoleName = roleName;
                string userData = JsonConvert.SerializeObject(userDto);
                HttpContext.Session.SetString("userData", userData);
            }
            return RedirectToAction("Index", "Admin", new { area = "Attorney" });
        }
        public IActionResult ManageUser()
        {
            string ParentId = string.Empty;
            string userData = HttpContext.Session.GetString("userData");
            if (!string.IsNullOrEmpty(userData))
            {
                UserDto userDto = JsonConvert.DeserializeObject<UserDto>(userData);
                ParentId = userDto.ParentId;

                SResponse resp = Fetch.GotoService("api", $"UserManagement/GetUsersForAttorney?ParentId={ParentId}", "GET");
                if (resp.ErrorCode == 401)
                    return RedirectToAction("Login", "Account", new { area = "Security" });

                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    List<AspNetUsers> users = JsonConvert.DeserializeObject<List<AspNetUsers>>(resp.Resp);
                    if (users.Count() > 0)
                    {
                        users.ElementAt(0).CurrentUserEmail = userDto.Email;
                    }
                    return View(users);
                }
                else
                {
                    if (resp.Resp == "Please Add Firm Details First")
                    {
                        TempData["response"] = "Please Add Firm Details First";
                        return RedirectToAction("AddFirm", "Admin", new { area = "Attorney" });
                    }
                    else
                    {
                        TempData["response"] = resp.Resp;
                        return View();
                    }
                }
            }
            return View();
        }

        public IActionResult BlockUsers()
        {
            string userId = string.Empty;
            string userdto = HttpContext.Session.GetString("userData");
            UserDto userData = JsonConvert.DeserializeObject<UserDto>(userdto);
            if (userData != null)
                userId = userData.Id;
            SResponse resp = Fetch.GotoService("api", $"UserManagement/GetBlockedUsers?ParentId={userId}", "GET");
            if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
            {
                List<AspNetUsers> users = JsonConvert.DeserializeObject<List<AspNetUsers>>(resp.Resp);
                return View(users);
            }
            else
                return View();
        }
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ChangePassword(ChangePassword model)
        {
            try
            {
                string userdto = HttpContext.Session.GetString("userData");
                User userDto = new User();
                if (!string.IsNullOrEmpty(userdto))
                {
                    userDto = JsonConvert.DeserializeObject<User>(userdto);
                    userDto.newPassword = model.PasswordHash;
                }

                var body = JsonConvert.SerializeObject(userDto);
                //SResponse resp = AbsolCase.Configuration.RequestSender.Fetch.GotoService("api", $"UserManagement/ChangePassword?model={userDto}&newPassword={model.PasswordHash}", "Get");
                SResponse resp = Fetch.GotoService("api", $"UserManagement/ChangePassword", "POST", body);
                if (resp.Status && (resp.Resp != null))
                {
                    return RedirectToAction("ChangePassword");
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
        #endregion

        #region Activity Logs
        public IActionResult ActivityIndex()
        {
            SResponse resp = Fetch.GotoService("api", "Activity/GetActivity", "GET");
            if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
            {
                List<AdminActivity> activities = JsonConvert.DeserializeObject<List<AdminActivity>>(resp.Resp);
                return View(activities);
            }
            else
                return View();
        }
        #endregion

        #region Firm
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
            catch (Exception ex)
            {
                throw ex;
            }
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
                    return View(_resultModel.Where(x => x.RoleName != "Client").ToList());
                }
                if (resp.Resp == "Please Add Firm Details First")
                {
                    TempData["response"] = "Please Add Firm Details First";
                    return RedirectToAction("AddFirm", "Admin", new { area = "Attorney" });
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
        #endregion

        #region Finance
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

        #endregion

        #region News
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
        #endregion

        #region Reporting
        public IActionResult ClientReceipt()
        {
            return View();
        }
        public IActionResult ExpenseReceipt()
        {
            return View();
        }
        [HttpPost]
        public JsonResult SetClientTransactionList(List<ClientTransactionViewModel> ClientTransaction)
        {
            var SerializeObjectList = JsonConvert.SerializeObject(ClientTransaction);
            HttpContext.Session.SetString("ClientTransactions", SerializeObjectList);
            return Json("ok");
        }
        [HttpPost]
        public JsonResult SetExpenseList(List<JsonExpenseViewModel> Expenses)
        {
            var SerializeObjectExpenseList = JsonConvert.SerializeObject(Expenses);
            HttpContext.Session.SetString("Expenses", SerializeObjectExpenseList);
            return Json("ok");
        }
        [HttpGet]
        public IActionResult GenerateReceipt()
        {
            string ClientTransactions = HttpContext.Session.GetString("ClientTransactions");
            List<ClientTransactionViewModel> ClientTransactionsList = JsonConvert.DeserializeObject<List<ClientTransactionViewModel>>(ClientTransactions);
            return new ViewAsPdf(ClientTransactionsList);
        }
        [HttpGet]
        public IActionResult GenerateExpensesReceipt()
        {
            string Expenses = HttpContext.Session.GetString("Expenses");
            List<JsonExpenseViewModel> ExpenseList = JsonConvert.DeserializeObject<List<JsonExpenseViewModel>>(Expenses);
            return new ViewAsPdf(ExpenseList);
        }
        [HttpGet]
        public IActionResult Ledger()
        {
            return View();
        }

        public IActionResult GetTransactions()
        {
            SResponse resp = Fetch.GotoService("api", $"Transaction/GetTransactionsBetweenDates", "GET");
            if (resp.ErrorCode == 401)
                return RedirectToAction("Login", "Account", new { area = "Security" });

            if (resp.Status && !string.IsNullOrEmpty(resp.Resp))
            {
                List<Transactions> clientTransaction = JsonConvert.DeserializeObject<List<Transactions>>(resp.Resp);
                return PartialView("~/Views/PartialViews/_Ledger.cshtml", clientTransaction);
            }
            else
                return View();
        }


        public IActionResult Reporting()
        {
            return View();
        }

        public IActionResult AllClientInvoices()
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", $"UserManagement/GetUsersClient", "GET");
                if (resp.ErrorCode == 401)
                    return RedirectToAction("Login", "Account", new { area = "Security" });

                if (resp.Status && !string.IsNullOrEmpty(resp.Resp))
                {
                    List<Contact> clients = JsonConvert.DeserializeObject<List<Contact>>(resp.Resp);
                    return View(clients);
                }
                else
                    return View();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public IActionResult AllCasesInvoices()
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", $"UserManagement/GetCasesClient", "GET");
                if (resp.ErrorCode == 401)
                    return RedirectToAction("Login", "Account", new { area = "Security" });

                if (resp.Status && !string.IsNullOrEmpty(resp.Resp))
                {
                    List<CaseDetail> caseDetails = JsonConvert.DeserializeObject<List<CaseDetail>>(resp.Resp);
                    return View(caseDetails);
                }
                else
                    return View();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region RejectReason
        public IActionResult AddRejectReason()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddRejectReason(HireReason model)
        {
            try
            {
                var body = JsonConvert.SerializeObject(model);
                SResponse resp = Fetch.GotoService("api", "AttorneyAdmin/AddReason", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ManageRejectReason");
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
        public IActionResult UpdateReason(int Id)
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", $"AttorneyAdmin/GetReasonsById?Id={Id}", "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    HireReason model = JsonConvert.DeserializeObject<HireReason>(resp.Resp);
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
        public IActionResult UpdateReason(HireReason model)
        {
            try
            {
                var body = JsonConvert.SerializeObject(model);
                SResponse resp = Fetch.GotoService("api", "AttorneyAdmin/UpdateRejectReason", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ManageRejectReason");
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
        public IActionResult DeleteReason(int Id)
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", $"AttorneyAdmin/DeleteRejectReason?Id={Id}", "DELETE");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ManageRejectReason");
                }
                return RedirectToAction("ManageRejectReason");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IActionResult ManageRejectReason()
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", "AttorneyAdmin/GetReasons", "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    List<HireReason> hireReasons = JsonConvert.DeserializeObject<List<HireReason>>(resp.Resp);
                    return View(hireReasons);
                }
                else
                    return View();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region Lead Status
        public IActionResult AddLeadStatus()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddLeadStatus(LeadStatus model)
        {
            try
            {
                var body = JsonConvert.SerializeObject(model);
                SResponse resp = Fetch.GotoService("api", "AttorneyAdmin/AddLeadStatus", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ManageLeadStatus");
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
        public IActionResult UpdateLeadStatus(int Id)
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", $"AttorneyAdmin/GetLeadStatusId?Id={Id}", "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    LeadStatus model = JsonConvert.DeserializeObject<LeadStatus>(resp.Resp);
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
        public IActionResult UpdateLeadStatus(LeadStatus model)
        {
            try
            {
                var body = JsonConvert.SerializeObject(model);
                SResponse resp = Fetch.GotoService("api", "AttorneyAdmin/UpdateLeadStatus", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ManageLeadStatus");
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
        public IActionResult DeleteLeadStatus(int Id)
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", $"AttorneyAdmin/DeleteLeadStatus?Id={Id}", "DELETE");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ManageLeadStatus");
                }
                return RedirectToAction("ManageLeadStatus");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IActionResult ManageLeadStatus()
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", "AttorneyAdmin/GetLeadStatus", "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    List<LeadStatus> leadStatuses = JsonConvert.DeserializeObject<List<LeadStatus>>(resp.Resp);
                    return View(leadStatuses);
                }
                else
                    return View();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region Timeline
        public IActionResult TimelineIndex()
        {
            string userId = string.Empty;
            string userdto = HttpContext.Session.GetString("userData");
            UserDto userData = JsonConvert.DeserializeObject<UserDto>(userdto);
            if (userData != null)
                userId = userData.Id;
            SResponse resp = Fetch.GotoService("api", $"TimeLine/GetTimeLinesByUserId?userId={userId}", "GET");
            if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
            {
                List<Timeline> timelines = JsonConvert.DeserializeObject<List<Timeline>>(resp.Resp);
                if (timelines != null && timelines.Count > 0)
                {
                    foreach (var timeline in timelines)
                    {
                        if (!string.IsNullOrEmpty(timeline.FilePath) && !string.IsNullOrEmpty(timeline.FilePath.Trim()))
                            timeline.FilePath = $"{configuration.GetValue<string>("App:RemoteServerUrl")}{timeline.FilePath}";
                        if (!string.IsNullOrEmpty(timeline.DocFilePath) && !string.IsNullOrEmpty(timeline.DocFilePath.Trim()))
                            timeline.DocFilePath = $"{configuration.GetValue<string>("App:RemoteServerUrl")}{timeline.DocFilePath}";
                        if (!string.IsNullOrEmpty(timeline.VideoFilePath) && !string.IsNullOrEmpty(timeline.VideoFilePath.Trim()))
                            timeline.VideoFilePath = $"{configuration.GetValue<string>("App:RemoteServerUrl")}{timeline.VideoFilePath}";
                        if (!string.IsNullOrEmpty(timeline.UserImagePath) && !string.IsNullOrEmpty(timeline.UserImagePath.Trim()))
                            timeline.UserImagePath = $"{configuration.GetValue<string>("App:RemoteServerUrl")}{timeline.UserImagePath}";
                        else
                            timeline.UserImagePath = $"{configuration.GetValue<string>("App:RemoteServerUrl")}Images/blank-profile.png";
                    }
                }
                return View(timelines);
            }
            else
            {
                if (resp.Resp == "Please Add Firm Details First")
                {
                    TempData["response"] = "Please Add Firm Details First";
                    return RedirectToAction("AddFirm", "Admin", new { area = "Attorney" });
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
        public IActionResult Zoom(Timeline timeline)
        {
            var starttime = timeline.starttime;
            var duration = timeline.duration;
            var topic = timeline.topic;
            var timezone = timeline.timezone;
            if (timezone == "1")
            {
                timezone = "Asia/Tashkent";
            }
            else if (timezone == "2")
            {
                timezone = "Asia/Riyadh";
            }
            else if (timezone == "3")
            {
                timezone = "America/Los_Angeles";
            }
            //string dateTime =Convert.ToDateTime(starttime);
            starttime = starttime + ":00Z";
            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var now = DateTime.UtcNow;
            //var apiSecret = "Your API secret";
            var apiSecret = "QBBiGJkMGiAFuBEdUiXrEO4YxgIpFADB19Vs";
            byte[] symmetricKey = Encoding.ASCII.GetBytes(apiSecret);





            var tokenDescriptor = new SecurityTokenDescriptor
            {
                //Issuer = "Your API Key",
                Issuer = "Wa--b6UgQMGDrlDO3ZFoJg",
                Expires = now.AddSeconds(300),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256),
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);





            var client = new RestClient("https://api.zoom.us/v2/users/absolzoom@gmail.com/meetings");
            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;
            //request.AddJsonBody(new { topic = "Meeting with Ussain", agenda="agenda", duration = "10", start_time = "2021-05-20T05:00:00", type = "2" });
            request.AddJsonBody(new { topic = topic, duration = duration, start_time = starttime, auto_recording = "local", jbh_time = "0", join_before_host = "true", timezone = timezone, type = "2" });
            request.AddHeader("authorization", String.Format("Bearer {0}", tokenString));



            IRestResponse restResponse = client.Execute(request);
            HttpStatusCode statusCode = restResponse.StatusCode;
            int numericStatusCode = (int)statusCode;
            var jObject = JObject.Parse(restResponse.Content);
            Zoom zoomModel = new Zoom();
            zoomModel.Host = (string)jObject["start_url"];
            zoomModel.Join = (string)jObject["join_url"];
            zoomModel.Code = Convert.ToString(numericStatusCode);

            return Json(zoomModel);
        }

        [HttpGet]
        public IActionResult ZoomMeetingGet()
        {

            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var now = DateTime.UtcNow;
            //var apiSecret = "Your API secret";
            var apiSecret = "QBBiGJkMGiAFuBEdUiXrEO4YxgIpFADB19Vs";
            byte[] symmetricKey = Encoding.ASCII.GetBytes(apiSecret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                //Issuer = "Your API Key",
                Issuer = "Wa--b6UgQMGDrlDO3ZFoJg",
                Expires = now.AddSeconds(300),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256),
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            var from = DateTime.Now.AddDays(-14).ToString("yyyy-MM-dd");
            //var to = DateTime.Now.AddDays(+3).ToString("yyyy-MM-dd");
            var to = DateTime.Now.ToString("yyyy-MM-dd");
            var client = new RestClient("https://api.zoom.us/v2/users/absolzoom@gmail.com/recordings?page_size=30&from=" + from + "&to=" + to + "");


            var request = new RestRequest(Method.GET);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("authorization", String.Format("Bearer {0}", tokenString));



            IRestResponse restResponse = client.Execute(request);
            HttpStatusCode statusCode = restResponse.StatusCode;
            int numericStatusCode = (int)statusCode;
            var jObjectdata = JObject.Parse(restResponse.Content);



            List<Zoom> zoomModel = new List<Zoom>();

            var data = jObjectdata.ToString();

            var newdata = JsonConvert.DeserializeObject<Meetingresult>(data);

            for (int i = 0; i < newdata.meetings.Count(); i++)
            {
                Zoom obj = new Zoom();
                //var stringg = Convert.ToString(newdata.meetings[i].share_url);
                //zoomModel[i].shareurl = stringg;
                obj.meetingId = newdata.meetings[i].id;

                var tokenHandlerr = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                var noww = DateTime.UtcNow;
                //var apiSecret = "Your API secret";
                var apiSecrett = "QBBiGJkMGiAFuBEdUiXrEO4YxgIpFADB19Vs";
                byte[] symmetricKeyy = Encoding.ASCII.GetBytes(apiSecrett);
                var tokenDescriptorr = new SecurityTokenDescriptor
                {
                    //Issuer = "Your API Key",
                    Issuer = "Wa--b6UgQMGDrlDO3ZFoJg",
                    Expires = noww.AddSeconds(300),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKeyy), SecurityAlgorithms.HmacSha256),
                };
                var tokenn = tokenHandlerr.CreateToken(tokenDescriptorr);
                var tokenStringg = tokenHandlerr.WriteToken(tokenn);

                var getmeeting = new RestClient("https://api.zoom.us/v2/meetings/" + obj.meetingId + "/recordings");
                //var getmeeting = new RestClient("https://api.zoom.us/v2/meetings/86537176557/recordings");
                var newrequest = new RestRequest(Method.GET);
                newrequest.RequestFormat = DataFormat.Json;
                newrequest.AddHeader("authorization", String.Format("Bearer {0}", tokenStringg));
                IRestResponse restResponsee = getmeeting.Execute(newrequest);
                var getmeetingdata = JObject.Parse(restResponsee.Content);
                var meetinggetdata = getmeetingdata.ToString();
                var meetingDetails = JsonConvert.DeserializeObject<meetingDetails>(meetinggetdata);

                obj.shareurl = meetingDetails.share_url;
                obj.passcode = meetingDetails.password;
                obj.title = meetingDetails.topic;

                zoomModel.Add(obj);
            }




            return Json(zoomModel);
        }
        #endregion

        #region TimeEntry
        public IActionResult ManageTimeEntryActivity()
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", "AttorneyAdmin/GetTimeEntryActivity", "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    List<TimeEntryActivity> data = JsonConvert.DeserializeObject<List<TimeEntryActivity>>(resp.Resp);
                    return View(data);
                }
                else
                    return View();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public IActionResult UpdateTimeEntryActivity(int Id)
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", $"AttorneyAdmin/GetTimeEntryActivityId?Id={Id}", "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    TimeEntryActivity model = JsonConvert.DeserializeObject<TimeEntryActivity>(resp.Resp);
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
        public IActionResult UpdateTimeEntryActivity(TimeEntryActivity model)
        {
            try
            {
                var body = JsonConvert.SerializeObject(model);
                SResponse resp = Fetch.GotoService("api", "AttorneyAdmin/UpdateTimeEntryActivity", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ManageTimeEntryActivity");
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

        public IActionResult ManageTimeEntry()
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", "AttorneyAdmin/GetTimeEntry", "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    List<TimeEntry> data = JsonConvert.DeserializeObject<List<TimeEntry>>(resp.Resp);
                    return View(data);
                }
                else
                    return View();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion


    }
}
