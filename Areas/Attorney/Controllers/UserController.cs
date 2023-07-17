using AbsolCase.Configurations;
using AbsolCase.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static AbsolCase.Configuration.RequestSender;

namespace AbsolCase.Areas.Attorney.Controllers
{
    [ServiceFilter(typeof(ConfigureSession))]
    public class UserController : Controller
    {
        private readonly IConfiguration config;
        public UserController(IConfiguration config)
        {
            this.config = config;
        }
        public IActionResult Index()
        {
            return View();
        }
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
                    return RedirectToAction("AddFirm", "Firm", new { area = "Attorney" });
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
                        model.ImagePath = $"{config.GetValue<string>("App:RemoteServerUrl")}{model.ImagePath}";
                    }
                    else
                    {
                        model.ImagePath = $"{config.GetValue<string>("App:RemoteServerUrl")}Images/blank-profile.png";
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
                    userDto.ImagePath = $"{config.GetValue<string>("App:RemoteServerUrl")}{userDto.ImagePath}";
                else
                    userDto.ImagePath = $"{config.GetValue<string>("App:RemoteServerUrl")}Images/blank-profile.png";

                userDto.Token = token;
                userDto.RoleName = roleName;
                string userData = JsonConvert.SerializeObject(userDto);
                HttpContext.Session.SetString("userData", userData);
            }
            return RedirectToAction("Index", "AttorneyDashboard", new { area = "Attorney" });
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
                    if (users.Count()>0)
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
                    return RedirectToAction("AddFirm", "Firm", new { area = "Attorney" });
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
    }
}
