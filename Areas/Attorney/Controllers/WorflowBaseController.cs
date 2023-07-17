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
    public class WorkflowBaseController : Controller
    {

        public IActionResult Index()
        {
            SResponse taskresp = Fetch.GotoService("api", "Workflow/GetAllWorkflows", "GET");
            if (taskresp.Status && (taskresp.Resp != null) && (taskresp.Resp != ""))
            {
                List<WorkflowBase> task = JsonConvert.DeserializeObject<List<WorkflowBase>>(taskresp.Resp);
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
        public IActionResult Detail(int id)
        {
            try
            {
                SResponse taskresp = Fetch.GotoService("api", $"Workflow/GetWorkflowById?Id={id}", "GET");
                if (taskresp.Status && (taskresp.Resp != null) && (taskresp.Resp != ""))
                {

                    WorkflowBase data = JsonConvert.DeserializeObject<WorkflowBase>(taskresp.Resp);
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
        public IActionResult UpdateWorkflowTask(int Id)
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

        #endregion


        #region calender

        public IActionResult WorkflowEvents()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetWorkflowEvents(int id)
        {
            string userId = string.Empty;
            string userData = HttpContext.Session.GetString("userData");
            if (!string.IsNullOrEmpty(userData))
            {
                UserDto userDto = JsonConvert.DeserializeObject<UserDto>(userData);
                userId = userDto.ParentId;
            }
            SResponse resp = Fetch.GotoService("api", $"Events/GetWorkflowEvents?userId={userId}&id={id}", "GET");
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
                    model.UserId = userDto.ParentId;
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

        #endregion

        #region Document

        #endregion
    }
}
