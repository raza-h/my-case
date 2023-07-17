using AbsolCase.Configurations;
using AbsolCase.Models;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Rotativa.AspNetCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using static AbsolCase.Configuration.RequestSender;

namespace AbsolCase.Areas.Administration.Controllers
{
    [ServiceFilter(typeof(ConfigureSession))]
    public class UsersController : Controller
    {
        private IConverter _converter;
        private readonly IConfiguration config;
        public UsersController(IConverter converter, IConfiguration config)
        {
            _converter = converter;
            this.config = config;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddUser(UserSignupDto user)
        {
            try
            {
                if (user.profile_img != null && user.profile_img.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        user.profile_img.CopyTo(ms);
                        user.file = ms.ToArray();
                        user.profile_img = null;
                    }
                }

                string userdto = HttpContext.Session.GetString("userData");
                UserDto userData = JsonConvert.DeserializeObject<UserDto>(userdto);
                if (userData != null)
                    user.ParentId = userData.Id;
                var body = JsonConvert.SerializeObject(user);
                SResponse resp = Fetch.GotoService("api", "UserManagement/InviteMember", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    if (resp.Resp == "User already exist")
                    {
                        TempData["response"] = resp.Resp;
                        return RedirectToAction("AddUser");
                    }
                    return RedirectToAction("ManageUser");
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

        public IActionResult DeleteUser(string Id)
        {
            SResponse resp = Fetch.GotoService("api", $"UserManagement/DeleteUser?Id={Id}", "DELETE");
            return RedirectToAction("ManageUser");
        }

        public IActionResult UpdateUser(string Id)
        {
            SResponse resp = Fetch.GotoService("api", $"UserManagement/GetUserById?Id={Id}", "GET");
            AspNetUsers user = JsonConvert.DeserializeObject<AspNetUsers>(resp.Resp);

            SResponse response = Fetch.GotoService("api", "UserManagement/GetRoles", "GET");
            if (response.Status && (response.Resp != null) && (response.Resp != ""))
            {
                List<AspNetRoles> roles = JsonConvert.DeserializeObject<List<AspNetRoles>>(response.Resp).ToList();
                ViewBag.RolesList = roles.Select(c => c.Name).ToList();
            }

            return View(user);
        }

        [HttpPost]
        public IActionResult UpdateUser(AspNetUsers user)
        {
            try
            {
                if (user.profile_img != null && user.profile_img.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        user.profile_img.CopyTo(ms);
                        user.file = ms.ToArray();
                        user.profile_img = null;
                    }
                }
                var body = JsonConvert.SerializeObject(user);
                SResponse resp = Fetch.GotoService("api", "UserManagement/UpdateUser", "POST", body);
                if (resp.Status && (resp.Resp != null))
                {
                    return RedirectToAction("ManageUser");
                }
                else
                {
                    TempData["response"] = resp.Resp + " " + "Unable To Update User";
                    return RedirectToAction("UpdateUser");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public IActionResult ManageUser()
        {
            string userId = string.Empty;
            string userdto = HttpContext.Session.GetString("userData");
            UserDto userData = JsonConvert.DeserializeObject<UserDto>(userdto);
            if (userData != null)
                userId = userData.Id;
            SResponse resp = Fetch.GotoService("api", $"Administrator/GetAdminUsers?ParentId={userId}", "GET");
            if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
            {
                List<AspNetUsers> users = JsonConvert.DeserializeObject<List<AspNetUsers>>(resp.Resp);
                users.ElementAt(0).CurrentUserEmail = userData.Email;
                return View(users);
            }
            else
                return View();
        }
        public IActionResult BlockUser()
        {
            string userId = string.Empty;
            string userdto = HttpContext.Session.GetString("userData");
            UserDto userData = JsonConvert.DeserializeObject<UserDto>(userdto);
            if (userData != null)
                userId = userData.Id;
            SResponse resp = Fetch.GotoService("api", $"UserManagement/GetBlockedUsers?ParentId={userId}", "GET");
            if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
            {
                List<AspNetUsers> users = JsonConvert.DeserializeObject<List<AspNetUsers>>(resp.Resp);
                return View(users);
            }
            else
                return View();
        }
        public IActionResult UserProfile()
        {
            string userdto = HttpContext.Session.GetString("userData");
            User model = new User();
            UserDto userDto = new UserDto();
            if (!string.IsNullOrEmpty(userdto))
            {
                userDto = JsonConvert.DeserializeObject<UserDto>(userdto);
                SResponse resp = Fetch.GotoService("api", $"UserManagement/GetUserById?Id={userDto.Id}", "GET");
                if (resp.Status && (!string.IsNullOrEmpty(resp.Resp)))
                {
                    model = JsonConvert.DeserializeObject<User>(resp.Resp);
                    if (!string.IsNullOrEmpty(model.ImagePath) && !string.IsNullOrEmpty(model.ImagePath.Trim()))
                    {
                        ViewBag.profileImage = model.ImagePath;
                        model.ImagePath = $"{config.GetValue<string>("App:RemoteServerUrl")}{model.ImagePath}";
                        userDto.ImagePath = model.ImagePath;
                        HttpContext.Session.SetString("userData", JsonConvert.SerializeObject(userDto));
                    }
                    else
                    {
                        model.ImagePath = $"{config.GetValue<string>("App:RemoteServerUrl")}Images/blank-profile.png";
                    }
                }
            }
            return View(model);
        }
        [HttpPost]
        public IActionResult UserProfile(User model)
        {
            if (model.Image != null && model.Image.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    model.Image.CopyTo(ms);
                    model.File = ms.ToArray();
                    model.Image = null;
                }
            }
            var body = JsonConvert.SerializeObject(model);
            SResponse resp = Fetch.GotoService("api", $"UserManagement/UpdateUser", "POST", body);
            if (resp.Status && (!string.IsNullOrEmpty(resp.Resp)))
            {
                string token = string.Empty;
                string roleName = string.Empty;
                UserDto sessionData = new UserDto();
                string sessionString = HttpContext.Session.GetString("userData");
                if (!string.IsNullOrEmpty(sessionString))
                {
                    sessionData = JsonConvert.DeserializeObject<UserDto>(sessionString);
                    token = sessionData.Token;
                    roleName = sessionData.RoleName;
                }

                HttpContext.Session.Remove("userData");
                UserDto userDto = JsonConvert.DeserializeObject<UserDto>(resp.Resp);
                if (!string.IsNullOrEmpty(userDto.ImagePath) && !string.IsNullOrEmpty(userDto.ImagePath.Trim()))
                    userDto.ImagePath = $"{config.GetValue<string>("App:RemoteServerUrl")}{userDto.ImagePath}";
                else
                    userDto.ImagePath = $"{config.GetValue<string>("App:RemoteServerUrl")}Images/blank-profile.png";

                userDto.Token = token;
                userDto.RoleName = roleName;
                string userData = JsonConvert.SerializeObject(userDto);
                HttpContext.Session.SetString("userData", userData);
            }
            return RedirectToAction("Index", "Dashboard", new { area = "Administration" });
        }
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ChangePassword(ChangePassword model)
        {
            try
            {
                string userdto = HttpContext.Session.GetString("userData");
                User userDto = new User();
                if (!string.IsNullOrEmpty(userdto))
                {
                    userDto = JsonConvert.DeserializeObject<User>(userdto);
                    userDto.newPassword = model.PasswordHash;
                }

                var body = JsonConvert.SerializeObject(userDto);
                SResponse resp = AbsolCase.Configuration.RequestSender.Fetch.GotoService("api", $"UserManagement/ChangePassword", "POST", body);
                if (resp.Status && (resp.Resp != null))
                {
                    TempData["response"] ="Password changed successfully ";

                    return RedirectToAction("ChangePassword");
                }
                else
                {
                    TempData["response"] = resp.Resp + " " + "Unable To Update User";
                    return RedirectToAction("ChangePassword");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet]
        public IActionResult FilterByDates(string FromDate, string ToDate)
        {
            SResponse resp = Fetch.GotoService("api", $"Activity/GetActivity?IsFilterByDates=true&&Date1={FromDate}&&Date2={ToDate}", "GET");
            if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
            {
                List<AdminActivity> activities = JsonConvert.DeserializeObject<List<AdminActivity>>(resp.Resp);
                return PartialView("~/Views/PartialViews/_Activity.cshtml", activities);
            }
            return Ok();
        }
        public IActionResult AdminActivityLog()
        {
            SResponse resp = Fetch.GotoService("api", "Activity/GetActivity", "GET");
            if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
            {
                List<AdminActivity> activities = JsonConvert.DeserializeObject<List<AdminActivity>>(resp.Resp);
                return View(activities);
            }
            else
                return View();
            
        }
        //public IActionResult PendingCustomer()
        //{
        //    string status = "Pending";
        //    SResponse resppending = Fetch.GotoService("api", $"UserManagement/GetUsers?Status={status}", "GET");
        //    if (resppending.ErrorCode == 401)
        //        return RedirectToAction("Login", "Account", new { area = "Security" });

        //    else if (resppending.Status && (resppending.Resp != null) && (resppending.Resp != ""))
        //    {
        //        List<AspNetUsers> users = JsonConvert.DeserializeObject<List<AspNetUsers>>(resppending.Resp);
        //        return View(users);
        //    }
        //    else
        //        return View();
        //}
        public IActionResult PendingCustomer()
        {
           
            SResponse resppending = Fetch.GotoService("api", $"Administrator/GetPendingUsersCustomer", "GET");
            if (resppending.ErrorCode == 401)
                return RedirectToAction("Login", "Account", new { area = "Security" });

            else if (resppending.Status && (resppending.Resp != null) && (resppending.Resp != ""))
            {
                List<AspNetUsers> users = JsonConvert.DeserializeObject<List<AspNetUsers>>(resppending.Resp);
                return View(users);
            }
            else
                return View();
        }
        //public IActionResult ApprovedCustomer()
        //{
        //    string status = "Approved";
        //    SResponse respapproved = AbsolCase.Configuration.RequestSender.Fetch.GotoService("api", $"UserManagement/GetUsers?Status={status}", "GET");
        //    if (respapproved.Status && (respapproved.Resp != null) && (respapproved.Resp != ""))
        //    {

        //        List<AspNetUsers> users = JsonConvert.DeserializeObject<List<AspNetUsers>>(respapproved.Resp);
        //        return View(users);
        //    }
        //    else
        //        return View();
        //}
        public IActionResult ApprovedCustomer()
        {
            string status = "Approved";
            SResponse respapproved = AbsolCase.Configuration.RequestSender.Fetch.GotoService("api", $"Administrator/GetApprovedUsersCustomer?Status={status}", "GET");
            if (respapproved.Status && (respapproved.Resp != null) && (respapproved.Resp != ""))
            {

                List<AspNetUsers> users = JsonConvert.DeserializeObject<List<AspNetUsers>>(respapproved.Resp);
                return View(users);
            }
            else
                return View();
        }
        //public IActionResult RejectedCustomer()
        //{
        //    string status = "Rejected";
        //    SResponse resprejected = AbsolCase.Configuration.RequestSender.Fetch.GotoService("api", $"UserManagement/GetUsers?Status={status}", "GET");
        //    if (resprejected.Status && (resprejected.Resp != null) && (resprejected.Resp != ""))
        //    {
        //        List<AspNetUsers> users = JsonConvert.DeserializeObject<List<AspNetUsers>>(resprejected.Resp);
        //        return View(users);
        //    }
        //    else
        //        return View();
        //}
        public IActionResult RejectedCustomer()
        {
            string status = "Rejected";
            SResponse resprejected = AbsolCase.Configuration.RequestSender.Fetch.GotoService("api", $"Administrator/GetRejectedUsersCustomer?Status={status}", "GET");
            if (resprejected.Status && (resprejected.Resp != null) && (resprejected.Resp != ""))
            {
                List<AspNetUsers> users = JsonConvert.DeserializeObject<List<AspNetUsers>>(resprejected.Resp);
                return View(users);
            }
            else
                return View();
        }
        public IActionResult ViewCustomerDetail(string id)
        {
            SResponse resprejected = AbsolCase.Configuration.RequestSender.Fetch.GotoService("api", $"UserManagement/GetUserById?Id={id}", "GET");
            if (resprejected.Status && (resprejected.Resp != null) && (resprejected.Resp != ""))
            {
                AspNetUsers user = JsonConvert.DeserializeObject<AspNetUsers>(resprejected.Resp);
                return View(user);
            }
            else
                return View();
        }
        public IActionResult generatePDF()
        {
            var pdfResult = new ViewAsPdf()
            {
                CustomSwitches =
          "--footer-center \"  Printed Date: " +
        DateTime.Now.Date.ToString("dd/MM/yyyy") + "  Page: [page]/[toPage]\"" +
        " --footer-line --footer-font-size \"12\" --footer-spacing 1 --footer-font-name \"Segoe UI\""
            };
            //  {//      CustomSwitches =//  "--footer-center \"  Printed Date: " +//DateTime.Now.Date.ToString("dd/MM/yyyy") + "  Page: [page]/[toPage]\"" +//" --footer-line --footer-font-size \"12\" --footer-spacing 1 --footer-font-name \"Segoe UI\""//  };//  //return new ViewAsPdf(_resultModel);//  return pdfResult;
            return pdfResult;
        }

        [HttpPost]
        public IActionResult ChangeStatus(string Id, string status)
        {

            SResponse resprejected = AbsolCase.Configuration.RequestSender.Fetch.GotoService("api", $"UserManagement/ChangeStatus?Id={Id}&Status={status}", "GET");
            if (resprejected.Status)
            {
                if (status == "Approved")
                {
                    string path1 = @"wwwroot";
                    string path = Path.GetFullPath(path1);
                    var sb = new StringBuilder();
                    sb.Append(@"<html lang='en'><head><meta charset ='utf-8'><meta name ='viewport' content ='width=device-width, initial-scale=1, shrink-to-fit=no'><link href ='https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css' rel = 'stylesheet'><link rel = 'stylesheet' href = 'https://cdn.jsdelivr.net/npm/bootstrap-icons@1.5.0/font/bootstrap-icons.css'><link rel = 'stylesheet' href = '~/vendors/feather/feather.css'><link rel = 'stylesheet' href = '~/vendors/mdi/css/materialdesignicons.min.css'><link rel = 'stylesheet' href = '~/vendors/ti-icons/css/themify-icons.css' ><link rel = 'stylesheet' href = '~/vendors/typicons/typicons.css'><link rel = 'stylesheet' href = '~/vendors/simple-line-icons/css/simple-line-icons.css'><link rel = 'stylesheet' href = '~/vendors/css/vendor.bundle.base.css'><link rel = 'stylesheet' href = '~/vendors/datatables.net-bs4/dataTables.bootstrap4.css'><link rel = 'stylesheet' href = '~/js/select.dataTables.min.css' ><link rel = 'stylesheet' href = '~/css/vertical-layout-light/style.css'><link href = '~/assets/css/appdefault.css' rel = 'stylesheet' /><link href = '~/css/case.css' rel = 'stylesheet' /><link rel = 'stylesheet' href = 'https://cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/toastr.min.css' type = 'text/css' /></ head > ");
                    sb.Append(@"<div class='row'><h3> Your Last Invoice </h3><div class='col-lg-12'><div class='container-fluid'><h3 class='text-right my-5'>Invoice&nbsp;&nbsp;#INV-17</h3><hr></div><div class='container-fluid d-flex justify-content-between'><div class='col-lg-3 ps-0'><p class='mt-5 mb-2'><b>Star Admin2</b></p><p>104,<br>Minare SK,<br>USA, K1A 0G9.</p></div><div class='col-lg-3 pr-0'><p class='mt-5 mb-2 text-right'><b>Invoice to</b></p><p class='text-right'>Gaala &amp; Sons,<br> C-201, Beykoz-34800,<br> USA, K1A 0G9.</p></div></div><div class='container-fluid d-flex justify-content-between'><div class='col-lg-3 ps-0'><p class='mb-0 mt-5'>Invoice Date : 23rd Sep 2022</p><p>Due Date : 30th Sep 2022</p></div></div><div class='container-fluid mt-5 d-flex justify-content-center w-100'><div class='table-responsive w-100'><table class='table'><thead> <tr class='bg-dark text-white'><th>#</th><th>Description</th><th class='text-right'>Package</th><th class='text-right'>Duration</th><th class='text-right'>Total</th></tr></thead><tbody><tr class='text-right'><td class='text-left'>1</td><td class='text-left'>Basic</td><td>1 Month</td><td>$200</td></tr></tbody></table></div></div><div class='container-fluid mt-5 w-100'><p class='text-right mb-2'>Sub - Total amount: $2,00</p><p class='text-right'>vat(10%) : $18</p><h4 class='text-right mb-5'>Total : $2,18</h4><hr></div><div class='container-fluid w-100'></div></div></div>");

                    Random rnd = new Random();
                    string fullPath = path + "/Invoices/";

                    string fileName = rnd.Next(10, 100000).ToString() + ".pdf";
                    var globalSettings = new GlobalSettings
                    {
                        ColorMode = DinkToPdf.ColorMode.Color,
                        Orientation = Orientation.Portrait,
                        PaperSize = PaperKind.A4,
                        Margins = new MarginSettings { Top = 10 },
                        DocumentTitle = "PDF Report",
                        Out = fullPath + fileName,
                    };

                    var objectSettings = new ObjectSettings
                    {
                        PagesCount = true,
                        HtmlContent = sb.ToString(),
                        WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory() + "/style.css") },
                    };
                    var pdf = new HtmlToPdfDocument()
                    {
                        GlobalSettings = globalSettings,
                        Objects = { objectSettings }
                    };
                    var _pdf = _converter.Convert(pdf);
                    MemoryStream ms = new MemoryStream();
                    var fileInBytes = System.IO.File.ReadAllBytes(fullPath + fileName);
                    // Save and close the document.
                    try
                    {
                        MailMessage mail = new MailMessage();
                        SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                        mail.From = new MailAddress("solutionabsolute28@gmail.com");
                        mail.To.Add("rizwanahmed4642@gmail.com");
                        mail.Subject = "Invitation to sign up on CMS";
                        mail.Body = /*ConvertHtmlToString("Usman", "usman1@abs.com")*/ "Usman has invited you to sign up on CMS click <a href='#'>Complete Registration</a> to complete sign up";
                        mail.IsBodyHtml = true;
                        Attachment att = new Attachment(new MemoryStream(fileInBytes), fileName);
                        mail.Attachments.Add(att);
                        SmtpServer.Port = 587;
                        SmtpServer.UseDefaultCredentials = false;
                        SmtpServer.Credentials = new System.Net.NetworkCredential("solutionabsolute28@gmail.com", "absolcase123");
                        SmtpServer.EnableSsl = true;
                        SmtpServer.Send(mail);
                    }
                    catch (Exception ex)
                    {
                        //     return false;
                    }
                    ms.Position = 0;
                    return Json("ok");
                }
                return Json("ok");
            }
            else
                return View();
        }



    }
}
