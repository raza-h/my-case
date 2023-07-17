using AbsolCase.Configurations;
using AbsolCase.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Rotativa.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AbsolCase.Configuration.RequestSender;

namespace AbsolCase.Areas.Attorney.Controllers
{
    [ServiceFilter(typeof(ConfigureSession))]
    public class AdminNotesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ManageNotes()
        {
            try
            {

                string userId = string.Empty;
                string userdto = HttpContext.Session.GetString("userData");
                UserDto userData = JsonConvert.DeserializeObject<UserDto>(userdto);
                if (userData != null)
                    userId = userData.Id;
                SResponse resp = Fetch.GotoService("api", $"Notes/GetAdminNotesDetails?UserId={userId}", "GET");

                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    List<Notes> task = JsonConvert.DeserializeObject<List<Notes>>(resp.Resp);
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
        public IActionResult DownloadNotes(string NotesTag, string NotesTittle, String NotesDescripation)
        {
            try
            {
                Notes model = new Notes();
                model.NotesTag = NotesTag;
                model.NotesTittle = NotesTittle;
                model.NotesDescripation = NotesDescripation;
                var pdfResult = new ViewAsPdf(model);
                return pdfResult;

            }
            catch (Exception ex)
            {
                throw;
            }


        }
    }
}
