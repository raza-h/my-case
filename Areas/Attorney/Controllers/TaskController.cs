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
    public class TaskController : Controller
    {
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
                    return RedirectToAction("AddFirm", "Firm", new { area = "Attorney" });
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
                    return RedirectToAction("AddFirm", "Firm", new { area = "Attorney" });
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
    }
}
