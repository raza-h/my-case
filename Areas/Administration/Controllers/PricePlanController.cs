using AbsolCase.Configurations;
using AbsolCase.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using static AbsolCase.Configuration.RequestSender;

namespace AbsolCase.Areas.Public
{
    [ServiceFilter(typeof(ConfigureSession))]
    public class PricePlanController : Controller
    {
        public IActionResult Index()
        {
            SResponse resp = Fetch.GotoService("api", "PricePlan/GetPackages", "GET");
            if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
            {
                List<PackageService> package = JsonConvert.DeserializeObject<List<PackageService>>(resp.Resp);
                return View(package);
            }
            return View();
        }

        public IActionResult AddPricePlan()
        {
            SResponse response = Fetch.GotoService("api", "PricePlan/GetServices", "GET");
            if (response.Status && (response.Resp != null) && (response.Resp != ""))
            {
                List<Service> services = JsonConvert.DeserializeObject<List<Service>>(response.Resp).ToList();
                ViewBag.ServicesList = services;
            }
            return View();
        }

        [HttpPost]
        public IActionResult AddPricePlan(PricePlan pricePlan)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var body = JsonConvert.SerializeObject(pricePlan);
                    SResponse resp = Fetch.GotoService("api", "PricePlan/AddPricePlan", "POST", body);
                    if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                    {
                        return RedirectToAction("ManagePricePlans");
                    }
                    else
                    {
                        TempData["response"] = resp.Resp;
                        return RedirectToAction("AddPricePlan");
                    }
                }
                SResponse response = Fetch.GotoService("api", "PricePlan/GetServices", "GET");
                if (response.Status && (response.Resp != null) && (response.Resp != ""))
                {
                    List<Service> services = JsonConvert.DeserializeObject<List<Service>>(response.Resp).ToList();
                    ViewBag.ServicesList = services;
                }
                return View(pricePlan);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IActionResult UpdatePricePlan(int Id)
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", $"PricePlan/GetPricePlanById?Id={Id}", "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    PricePlan pricePlan = JsonConvert.DeserializeObject<PricePlan>(resp.Resp);
                    return View(pricePlan);
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
        public IActionResult UpdatePricePlan(PricePlan pricePlan)
        {
            try
            {
                var body = JsonConvert.SerializeObject(pricePlan);
                SResponse resp = Fetch.GotoService("api", "PricePlan/UpdatePricePlan", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ManagePricePlans");
                }
                else
                {
                    TempData["response"] = resp.Resp;
                    return View();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IActionResult DeletePricePlan(int Id)
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", $"PricePlan/DeletePricePlan?Id={Id}", "DELETE");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ManagePricePlans");
                }
                return RedirectToAction("ManagePricePlans");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IActionResult ManagePricePlans()
        {
            SResponse resp = Fetch.GotoService("api", "PricePlan/GetPricePlans", "GET");
            if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
            {
                List<PricePlan> pricePlans = JsonConvert.DeserializeObject<List<PricePlan>>(resp.Resp);
                return View(pricePlans);
            }
            else
                return View();
        }

        public IActionResult Details(int Id)
        {
            SResponse resp = Fetch.GotoService("api", $"PricePlan/GetPackageByPlanId?Id={Id}", "GET");
            if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
            {
                PackageService package = JsonConvert.DeserializeObject<PackageService>(resp.Resp);
                return View(package);
            }
            else
                return View();
        }

        public IActionResult AddService()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddService(Service service)
        {
            try
            {
                var body = JsonConvert.SerializeObject(service);
                SResponse resp = Fetch.GotoService("api", "PricePlan/AddService", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ManageServices");
                }
                else
                {
                    TempData["response"] = resp.Resp;
                    return View();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IActionResult ManageServices()
        {
            SResponse resp = Fetch.GotoService("api", "PricePlan/GetServices", "GET");
            if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
            {
                List<Service> service = JsonConvert.DeserializeObject<List<Service>>(resp.Resp);
                return View(service);
            }
            else
                return View();
        }
        public IActionResult UpdateService(int Id)
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", $"PricePlan/GetServiceById?Id={Id}", "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    Service service = JsonConvert.DeserializeObject<Service>(resp.Resp);
                    return View(service);
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
        public IActionResult UpdateService(Service service)
        {
            try
            {
                var body = JsonConvert.SerializeObject(service);
                SResponse resp = Fetch.GotoService("api", "PricePlan/UpdateService", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ManageServices");
                }
                else
                {
                    TempData["response"] = resp.Resp;
                    return View();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IActionResult DeleteService(int Id)
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", $"PricePlan/DeleteService?Id={Id}", "DELETE");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ManageServices");
                }
                return RedirectToAction("ManageServices");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IActionResult UpdatePackage(int Id)
        {
            SResponse response = Fetch.GotoService("api", "PricePlan/GetServices", "GET");
            if (response.Status && (response.Resp != null) && (response.Resp != ""))
            {
                List<Service> services = JsonConvert.DeserializeObject<List<Service>>(response.Resp).ToList();
                ViewBag.ServicesList = services;
            }

            SResponse resp = Fetch.GotoService("api", $"PricePlan/GetPackageByPlanId?Id={Id}", "GET");
            if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
            {
                PackageService packageService = JsonConvert.DeserializeObject<PackageService>(resp.Resp);
                packageService.PricePlanId = Id;
                return View(packageService);
            }
            else
                return View();
        }

        [HttpPost]
        public IActionResult UpdatePackage(PackageService packageService)
        {
            var body = JsonConvert.SerializeObject(packageService);
            SResponse resp = Fetch.GotoService("api", "PricePlan/UpdatePackage", "POST", body);
            if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
            {
                return RedirectToAction("ManagePricePlans");
            }
            else
            {
                TempData["response"] = resp.Resp;
                return View();
            }
        }
    }
}
