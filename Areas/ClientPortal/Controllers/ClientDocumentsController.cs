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
using System.Net;
using System.Threading.Tasks;
using static AbsolCase.Configuration.RequestSender;

namespace AbsolCase.Areas.ClientPortal.Controllers
{
    [ServiceFilter(typeof(ConfigureSession))]
    public class ClientDocumentsController : Controller
    {
        private readonly IConfiguration configuration;
        public ClientDocumentsController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ManageDocuments()
        {
            string userId = string.Empty;
            string userdto = HttpContext.Session.GetString("userData");
            UserDto userData = JsonConvert.DeserializeObject<UserDto>(userdto);
            if (userData != null)
                userId = userData.Id;
            SResponse resp = Fetch.GotoService("api", $"Documents/GetDocumentsClient?UserId={userId}", "GET");
            if (resp.ErrorCode == 401)
                return RedirectToAction("Login", "Account", new { area = "Security" });

            if (resp.Status && !string.IsNullOrEmpty(resp.Resp))
            {
                List<Decuments> _resultModel = JsonConvert.DeserializeObject<List<Decuments>>(resp.Resp);
                return View(_resultModel);
            }
            else
                return View();
            return View();
        }
        public IActionResult DownloadFiles(string id)
        {
            string fileExtention = "";
            string path = "";
            int docid = Convert.ToInt32(id);
            string _decumentPath = string.Empty;
            SResponse resp = AbsolCase.Configuration.RequestSender.Fetch.GotoService("api", $"Documents/GetDocumentsById?Id={id}", "GET");
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


    }
}
