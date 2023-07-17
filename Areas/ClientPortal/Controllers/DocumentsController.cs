using AbsolCase.Configurations;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AbsolCase.Areas.ClinetPortal.Controllers
{
    [ServiceFilter(typeof(ConfigureSession))]
    public class DocumentsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
