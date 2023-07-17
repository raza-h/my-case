using AbsolCase.Configurations;
using AbsolCase.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AbsolCase.Areas.Subscription.Controllers
{
    [ServiceFilter(typeof(ConfigureSession))]
    public class SubscribeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddSubscription()
        {
            return View();
        }
        public IActionResult ViewSubscription()
        {
            return View();
        }
        public IActionResult ManageSubscription()
        {
            return View();
        }
        public IActionResult ViewPlus()
        {
            return View();
        }
        public IActionResult ViewPro()
        {
            return View();
        }


    }
}
