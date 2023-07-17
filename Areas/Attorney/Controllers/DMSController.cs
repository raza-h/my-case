using AbsolCase.Configurations;
using AbsolCase.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MyCaseApi.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static AbsolCase.Configuration.RequestSender;

namespace AbsolCase.Areas.Attorney.Controllers
{
    [ServiceFilter(typeof(ConfigureSession))]
    public class DMSController : Controller
    {
        private readonly IConfiguration configuration;
        public DMSController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpGet]
        public IActionResult FolderIndex()
        {

            SResponse resp = Fetch.GotoService("api", $"DMS/GetFolders", "GET");
            if (resp.Status && resp.Resp != null && resp.Resp != "")
            {
                List<DocumentFolder> documentFolders = JsonConvert.DeserializeObject<List<DocumentFolder>>(resp.Resp);
                return View(documentFolders);
            }
            else
                return View();
        }
        [HttpGet]
        public IActionResult View(int? Gid, int? level)
        {
            try
            {
                string userId = string.Empty;
                string userdto = HttpContext.Session.GetString("userData");
                UserDto userData = JsonConvert.DeserializeObject<UserDto>(userdto);
                if (userData != null)
                    userId = userData.Id;
                SResponse resp = Fetch.GotoService("api", $"Documents/GetDocumentsDMS?Gid={Gid}&level={level}", "GET");
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
        [HttpGet]
        public IActionResult Versions(string token)
        {
            try
            {
                string userId = string.Empty;
                string userdto = HttpContext.Session.GetString("userData");
                UserDto userData = JsonConvert.DeserializeObject<UserDto>(userdto);
                if (userData != null)
                    userId = userData.Id;
                SResponse resp = Fetch.GotoService("api", $"Documents/GetDocumentsDMSbyVer?token={token}", "GET");
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


        //[HttpGet]
        //public IActionResult NewEdit(int? mainId, int? sub1Id, int? sub2Id, int? sub3Id)
        //{

        //    SResponse resp = Fetch.GotoService("api", $"DMS/GetAllFolders", "GET");
        //    if (resp.Status && resp.Resp != null && resp.Resp != "")
        //    {
        //        ViewBag.MainFolderId = mainId;
        //        ViewBag.SubFolder1Id = sub1Id;
        //        ViewBag.SubFolder2Id = sub2Id;
        //        ViewBag.SubFolder3Id = sub3Id;

        //        List<DocumentFolder> documentFolders = JsonConvert.DeserializeObject<List<DocumentFolder>>(resp.Resp);
        //        var data1 = documentFolders.Where(x => x.DocumentFolderId == mainId)?.FirstOrDefault();
        //        var data2 = documentFolders.ElementAt(0).DocSub1Folders.Where(x => x.Id == sub1Id)?.FirstOrDefault();
        //        var data3 = documentFolders.ElementAt(0).DocSub2Folders.Where(x => x.Id == sub2Id)?.FirstOrDefault();
        //        var data4 = documentFolders.ElementAt(0).DocSub3Folders.Where(x => x.Id == sub3Id)?.FirstOrDefault();

        //        ViewBag.MainFolderName = data1?.Name;
        //        ViewBag.Sub1FolderName = data2?.Name;
        //        ViewBag.Sub2FolderName = data3?.Name;
        //        ViewBag.Sub3FolderName = data4?.Name;

        //        return View();
        //    }
        //    else
        //        return View();
        //}

        //[HttpPost]
        //public IActionResult NewEditSave(int? mainId, int? sub1Id, int? sub2Id, int? sub3Id, string mainName, string sub1Name, string sub2Name, string sub3Name)
        //{
        //    try
        //    {
        //        DMSNewEditValidate data = new DMSNewEditValidate();
        //        data.mainId = mainId;
        //        data.sub1Id = sub1Id;
        //        data.sub2Id = sub2Id;
        //        data.sub3Id = sub3Id;
        //        data.mainName = mainName;
        //        data.sub1Name = sub1Name;
        //        data.sub2Name = sub2Name;
        //        data.sub3Name = sub3Name;

        //        var body = JsonConvert.SerializeObject(data);
        //        SResponse resp = Fetch.GotoService("api", "DMS/UpdateFolder", "POST", body);
        //        if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
        //        {
        //            return RedirectToAction("ManageNews");
        //        }
        //        else
        //        {
        //            TempData["response"] = resp.Resp;
        //            return View();
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }

        //    //await db.SaveChangesAsync();
        //    //return Json("success");
        //}

    }
}
