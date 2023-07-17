using AbsolCase.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static AbsolCase.Configuration.RequestSender;

namespace AbsolCase.Areas.Public.Controllers
{
    public class FAQSController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Faq()
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", "Faq/GetFaqs", "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    List<Faq> _resultModel = JsonConvert.DeserializeObject<List<Faq>>(resp.Resp);

                    return View(_resultModel);
                }
                else
                    return View();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
