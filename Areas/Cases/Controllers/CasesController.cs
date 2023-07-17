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

namespace AbsolCase.Areas.Cases.Controllers
{
    [ServiceFilter(typeof(ConfigureSession))]
    public class CasesController : Controller
    {
        public IActionResult AddContactGroup()
        {
         
            return View();
        }
        public IActionResult ManageContactGroup()
        {
            return View();
        }
        public IActionResult AddPracticeArea()
        {
            return View();
        }
        public IActionResult ManagePracticeArea()
        {
            return View();
        }
        public IActionResult AddUserTitle()
        {
            return View();
        }
        public IActionResult ManageUserTitle()
        {
            return View();
        }
        public IActionResult AddNotesTag()
        {
            return View();
        }
        public IActionResult ManageNotesTag()
        {
            return View();
        }
        public IActionResult AddDocumentTag()
        {
            return View();
        }
        public IActionResult ManageDocumentTag()
        {
            return View();
        }
        public IActionResult AddReferalSource()
        {
            return View();
        }
        public IActionResult ManageReferalSource()
        {
            return View();
        }
        public IActionResult ManageCases()
        {
            string ParentId = string.Empty;
            string userData = HttpContext.Session.GetString("userData");
            if (!string.IsNullOrEmpty(userData))
            {
                UserDto userDto = JsonConvert.DeserializeObject<UserDto>(userData);
                ParentId = userDto.Id;
            }
            List<CaseDetail> _resultModel = new List<CaseDetail>();
            SResponse responseForCase = Fetch.GotoService("api", $"Case/GetCasesCreatedBy?CreatedBy={ParentId}", "GET");
            if (responseForCase.Status && (responseForCase.Resp != null) && (responseForCase.Resp != ""))
            {
                _resultModel = JsonConvert.DeserializeObject<List<CaseDetail>>(responseForCase.Resp);
            }
            else
            {
                if (responseForCase.Resp == "Please Add Firm Details First")
                {
                    TempData["response"] = "Please Add Firm Details First";
                    return RedirectToAction("AddFirm", "Firm", new { area = "Attorney" });
                }
                else
                {
                    TempData["response"] = responseForCase.Resp;
                    return View();
                }
            }
            return View(_resultModel);
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

        
    }
}
