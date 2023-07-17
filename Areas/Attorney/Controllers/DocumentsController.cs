using AbsolCase.Configurations;
using AbsolCase.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
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
    public class DocumentsController : Controller
    {
        private readonly IConfiguration configuration;
        public DocumentsController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public IActionResult Index()
        {
            return View();
        }
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
                    return RedirectToAction("AddFirm", "Firm", new { area = "Attorney" });
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


    }
}
