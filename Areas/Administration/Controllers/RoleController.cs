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
    public class RoleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddRole()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddRole(AspNetRoles obj)
        {

            try
            {
                var body = JsonConvert.SerializeObject(obj);
                SResponse resp = Fetch.GotoService("api", "UserManagement/AddRole", "POST", body);
                if (resp.Status && (!string.IsNullOrEmpty(resp.Resp)))
                {
                    TempData["response"] = "Add Successfully";
                    return RedirectToAction("ManageRoles");
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
        public IActionResult UpdateRole()
        {
            return View();
        }
        public IActionResult ManageRoles()
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", "UserManagement/GetRoles", "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    List<AspNetRoles> roles = JsonConvert.DeserializeObject<List<AspNetRoles>>(resp.Resp);
                    return View(roles);
                }
                else
                {
                    TempData["response"] = resp.Resp + " " + "Unable To Add Role";
                    return RedirectToAction("Index","Home", new { area = "Public"});
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
