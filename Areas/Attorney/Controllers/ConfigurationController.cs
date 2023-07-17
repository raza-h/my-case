using AbsolCase.Configurations;
using AbsolCase.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static AbsolCase.Configuration.RequestSender;

namespace AbsolCase.Areas.Attorney.Controllers
{
    [ServiceFilter(typeof(ConfigureSession))]
    public class ConfigurationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
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
        [HttpPost]
        public IActionResult AddDocumentTag(DocumentTag model)
        {
            try
            {
                var body = JsonConvert.SerializeObject(model);
                SResponse resp = Fetch.GotoService("api", "ConfigManagement/AddDocumentTag", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ManageDocumentTag");
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
            catch (Exception)
            {
                throw;
            }
        }
        public IActionResult ManageDocumentTag()
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", "ConfigManagement/GetDocumentTag", "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    List<DocumentTag> _resultModel = JsonConvert.DeserializeObject<List<DocumentTag>>(resp.Resp);
                    return View(_resultModel);
                }
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
                SResponse resp = Fetch.GotoService("api", $"ConfigManagement/GetDocumentTagById?Id={Id}", "GET");
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
                SResponse resp = AbsolCase.Configuration.RequestSender.Fetch.GotoService("api", "ConfigManagement/UpdateDocumentTag", "POST", body);
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
                SResponse resp = Fetch.GotoService("api", $"ConfigManagement/DeleteDocumentTag?Id={Id}", "DELETE");
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
        [HttpPost]
        public IActionResult AddNotesTag(NotesTag model)
        {
            try
            {
                var body = JsonConvert.SerializeObject(model);
                SResponse resp = Fetch.GotoService("api", "ConfigManagement/AddNotesTag", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ManageNotesTag");
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
            catch (Exception)
            {
                throw;
            }
        }
        public IActionResult ManageNotesTag()   
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", "ConfigManagement/GetNotesTag", "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    List<NotesTag> _resultModel = JsonConvert.DeserializeObject<List<NotesTag>>(resp.Resp);
                    return View(_resultModel);
                }
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
                SResponse resp = Fetch.GotoService("api", $"ConfigManagement/GetNotesTagById?Id={Id}", "GET");
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
        public IActionResult DeleteNotesTag(int Id)
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", $"ConfigManagement/DeleteNotesTag?Id={Id}", "DELETE");
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
    }
}
