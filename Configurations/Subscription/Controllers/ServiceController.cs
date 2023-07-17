using AbsolCase.Configurations;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AbsolCase.Areas.Subscription.Controllers
{
    [ServiceFilter(typeof(ConfigureSession))]
    public class ServiceController : Controller
    {
        public IActionResult AddService()
        {
            return View();
        }
        public IActionResult AvailableServices()
        {
            return View();
        }
        public IActionResult AddNewService()
        {
            return View();
        }
        public IActionResult ManageService()
        {
            return View();
        }



    }
}
