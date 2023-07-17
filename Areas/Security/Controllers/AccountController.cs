using AbsolCase.Configurations;
using AbsolCase.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static AbsolCase.Configuration.RequestSender;

namespace AbsolCase.Areas.Security.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration config;
        public AccountController(IConfiguration config)
        {
            this.config = config;
        }

        public IActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginUser data)
        {
            try
            {
                var body = JsonConvert.SerializeObject(data);
                SResponse resp = Fetch.GotoService("api", "UserManagement/Login", "POST", body);
                if (resp.Status && !string.IsNullOrEmpty(resp.Resp))
                {
                    UserDto userDto = JsonConvert.DeserializeObject<UserDto>(resp.Resp);
                    if (!string.IsNullOrEmpty(userDto.ImagePath) && !string.IsNullOrEmpty(userDto.ImagePath.Trim()))
                        userDto.ImagePath = $"{config.GetValue<string>("App:RemoteServerUrl")}{userDto.ImagePath}";
                    else
                        userDto.ImagePath = $"{config.GetValue<string>("App:RemoteServerUrl")}Images/blank-profile.png";

                    HttpContext.Session.SetString("userData", JsonConvert.SerializeObject(userDto));
                    if (userDto.RoleName == "Admin")
                        return RedirectToAction("Index", "Dashboard", new { area = "Administration" });
                    else if (userDto.RoleName == "Attorney" || userDto.RoleName == "Customer")
                        //return RedirectToAction("Index", "AttorneyDashboard", new { area = "Attorney" });
                        return RedirectToAction("Index", "Admin", new { area = "Attorney" });
                    else if (userDto.RoleName == "Staff")
                        return RedirectToAction("Dashboard", "Dashboard", new { area = "Staff" });
                    else
                        return RedirectToAction("Index", "PortalHome", new { area = "ClientPortal" });
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
        public IActionResult SignUp()
        {
            return View();
        }

        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ForgetPassword(ForgetPassword forgetPassword)
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", $"UserManagement/ResetPassword?Email={forgetPassword.Email}", "GET");
                if (resp.Status)
                {
                    ViewBag.Message = "Email with password reset link has been sent to you with one hour validity, Please reset your password within hour";
                    return View();
                }
                else
                {
                    TempData["response"] = resp.Resp;
                    return View();
                }
            }
            catch (Exception ex)
            {
                TempData["response"] = ex.Message;
                return View();
            }
        }

        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ResetPassword(ForgetPassword forgetPassword)
        {
            var body = JsonConvert.SerializeObject(forgetPassword);
            SResponse resp = Fetch.GotoService("api", "UserManagement/ResetPassword", "POST", body);
            if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
            {
                return RedirectToAction("Login");
            }
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
