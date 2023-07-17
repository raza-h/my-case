using AbsolCase.Configuration;
using AbsolCase.Configurations;
using AbsolCase.Models;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Microsoft.Win32;
using Newtonsoft.Json;
using PayPal;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using static AbsolCase.Configuration.RequestSender;

namespace AbsolCase.Areas.Public.Controllers
{
    public class SubscriptionController : Controller
    {
        private readonly INotyfService _notyf;
        private readonly MyConfiguration _myConfiguration;
        public SubscriptionController(INotyfService notyf, IOptions<MyConfiguration> myConfiguration)
        {
            _notyf = notyf;
            _myConfiguration = myConfiguration.Value;
        }

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
        public IActionResult ReSubscribe()
        {
            SResponse resp = Fetch.GotoService("api", "PricePlan/GetPackages", "GET");
            if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
            {
                List<PackageService> packages = JsonConvert.DeserializeObject<List<PackageService>>(resp.Resp);
                string userdto = HttpContext.Session.GetString("userData");
                if (packages != null && packages.Count > 0 && !string.IsNullOrEmpty(userdto))
                {
                    UserDto userDto = JsonConvert.DeserializeObject<UserDto>(userdto);
                    if (userDto.PricePlanIds != null && userDto.PricePlanIds.Count > 0)
                        packages = packages.Where(x => !userDto.PricePlanIds.Contains(x.pricePlan.PlanID)).ToList();
                }
                return View(packages);
            }
            return View();
        }
        public IActionResult BillingMethod(int Id)
        {
            return View();
        }

        [HttpPost]
        public IActionResult BillingMethod(SignupDto signupDto)
        {
            try
            {
                var body = JsonConvert.SerializeObject(signupDto);
                SResponse resp = Fetch.GotoService("api", "UserManagement/register", "POST", body);
                if (resp.Status)
                {
                    return RedirectToAction("Index", "AttorneyDashboard", new { Area = "Attorney" });
                }
                else
                {
                    TempData["response"] = resp.Resp + " " + "Unable To register user";
                    return RedirectToAction("BillingMethod");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public IActionResult PaymentWithPaypal()
        {
            try
            {
                //getting the apiContext
                APIContext apiContext = PaypalConfiguration.GetAPIContext();
                //A resource representing a Payer that funds a payment Payment Method as paypal
                //Payer Id will be returned when payment proceeds or click to pay
                string payerId = HttpContext.Request.Query["PayerID"];

                if (string.IsNullOrEmpty(payerId))
                {
                    //this section will be executed first because PayerID doesn't exist
                    //it is returned by the create function call of the payment class

                    // Creating a payment
                    // baseURL is the url on which paypal sendsback the data.
                    string baseURI = $"{Request.Scheme}://{Request.Host.Value}" +
                                "/cms/Public/Subscription/PaymentWithPaypal?";

                    //here we are generating guid for storing the paymentID received in session
                    //which will be used in the payment execution

                    //CreatePayment function gives us the payment approval url
                    //on which payer is redirected for paypal account payment
                    var guid = Convert.ToString((new Random()).Next(100000));
                    var createdPayment = CreatePayment(apiContext, baseURI + "guid=" + guid);

                    //get links returned from paypal in response to Create function call

                    var links = createdPayment.links.GetEnumerator();

                    string paypalRedirectUrl = null;

                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;

                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment
                            paypalRedirectUrl = lnk.href;
                        }
                    }

                    // saving the paymentID in the key guid
                    HttpContext.Session.SetString(guid, createdPayment.id);

                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This function exectues after receving all parameters for the payment

                    var guid = HttpContext.Request.Query["guid"];
                    var email = "";
                    var password = "";
                    var executedPayment = ExecutePayment(apiContext, payerId, HttpContext.Session.GetString(guid));

                    //If executed payment failed then we will show payment failure message to user
                    if (executedPayment.state.ToLower() == "approved")
                    {
                        var body = HttpContext.Session.GetString("signupDto");
                        UserSignupDto userDto = JsonConvert.DeserializeObject<UserSignupDto>(body);
                        email =userDto.Email;
                        password=userDto.Password;
                        SResponse resp = Fetch.GotoService("api", "UserManagement/Register", "POST", body);
                        if (resp.Status && (!string.IsNullOrEmpty(resp.Resp)))
                        {
                            
                            return RedirectToAction("Login", "Account", new { area = "Security" });
                        }
                        else
                        {
                            return RedirectToAction("BillingMethod");
                        }
                    }
                    else
                        return RedirectToAction("BillingMethod");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet]
        public IActionResult ReSubscribeWithPaypal(int Id)
        {
            try
            {
                //getting the apiContext
                APIContext apiContext = PaypalConfiguration.GetAPIContext();
                //A resource representing a Payer that funds a payment Payment Method as paypal
                //Payer Id will be returned when payment proceeds or click to pay
                string payerId = HttpContext.Request.Query["PayerID"];

                if (string.IsNullOrEmpty(payerId))
                {
                    SResponse resp = Fetch.GotoService("api", $"PricePlan/GetPricePlanById?Id={Id}", "GET");
                    if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                    {
                        PricePlan pricePlan = JsonConvert.DeserializeObject<PricePlan>(resp.Resp);
                        HttpContext.Session.SetString("pricePlan", JsonConvert.SerializeObject(pricePlan));
                    }
                    //this section will be executed first because PayerID doesn't exist
                    //it is returned by the create function call of the payment class

                    // Creating a payment
                    // baseURL is the url on which paypal sendsback the data.
                    string baseURI = $"{Request.Scheme}://{Request.Host.Value}" +
                                "/cms/Public/Subscription/ReSubscribeWithPaypal?";

                    //here we are generating guid for storing the paymentID received in session
                    //which will be used in the payment execution

                    //CreatePayment function gives us the payment approval url
                    //on which payer is redirected for paypal account payment
                    var guid = Convert.ToString((new Random()).Next(100000));
                    var createdPayment = CreatePayment(apiContext, baseURI + "guid=" + guid);

                    //get links returned from paypal in response to Create function call

                    var links = createdPayment.links.GetEnumerator();

                    string paypalRedirectUrl = null;

                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;

                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment
                            paypalRedirectUrl = lnk.href;
                        }
                    }

                    // saving the paymentID in the key guid
                    HttpContext.Session.SetString(guid, createdPayment.id);

                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This function exectues after receving all parameters for the payment

                    var guid = HttpContext.Request.Query["guid"];

                    var executedPayment = ExecutePayment(apiContext, payerId, HttpContext.Session.GetString(guid));

                    //If executed payment failed then we will show payment failure message to user
                    if (executedPayment.state.ToLower() == "approved")
                    {
                        string price = HttpContext.Session.GetString("pricePlan");
                        PricePlan pricePlan = JsonConvert.DeserializeObject<PricePlan>(price);
                        if (pricePlan != null)
                            Id = pricePlan.PlanID;
                            SResponse response = Fetch.GotoService("api", $"UserManagement/SubscribePackage?planId={Id}", "POST");
                        if (response.Status && (!string.IsNullOrEmpty(response.Resp)))
                        {
                            HttpContext.Session.Clear();
                            return RedirectToAction("Login", "Account", new { area = "Security" });
                        }
                        else
                            return RedirectToAction("ReSubscribe");
                    }
                    else
                        return RedirectToAction("ReSubscribe");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public IActionResult PayWithPaypal([FromForm] SignupDto signupDto)
        {
            SResponse resp = Fetch.GotoService("api", $"PricePlan/GetPricePlanById?Id={signupDto.PricePlanId}", "GET");
            if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
            {
                PricePlan pricePlan = JsonConvert.DeserializeObject<PricePlan>(resp.Resp);
                HttpContext.Session.SetString("pricePlan", JsonConvert.SerializeObject(pricePlan));
                signupDto.PricePlan = pricePlan;
                HttpContext.Session.SetString("signupDto", JsonConvert.SerializeObject(signupDto));
                return Ok();
            }
            return View();
        }
        public IActionResult PaymentWithBank()
        {
            string userdto = HttpContext.Session.GetString("userDto");
            SignupDto signUpDto = JsonConvert.DeserializeObject<SignupDto>(userdto);
            ViewBag.Price = signUpDto != null && signUpDto.PricePlan != null ? signUpDto.PricePlan.PriceRange : "";
            return View();
        }
        [HttpPost]
        public IActionResult PaymentWithBank(IFormFile file)
        {
            string userdto = HttpContext.Session.GetString("userDto");
            SignupDto signUpDto = JsonConvert.DeserializeObject<SignupDto>(userdto);
            if (file != null && file.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    signUpDto.File = ms.ToArray();
                    file = null;
                }
            }
            var body = JsonConvert.SerializeObject(signUpDto);
            SResponse resp = AbsolCase.Configuration.RequestSender.Fetch.GotoService("api", "UserManagement/register", "POST", body);
            if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
            {
                ViewBag.Payment = "success";
                return View();
            }
            
            return View();
        }

        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution() { payer_id = payerId };
            Payment payment = new Payment() { id = paymentId };
            return payment.Execute(apiContext, paymentExecution);
        }
        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            Payment payment = new Payment();
            //create itemlist and add item objects to it
            var itemList = new ItemList() { items = new List<Item>() };
            //Adding Item Details like name, currency, price etc
            string price = HttpContext.Session.GetString("pricePlan");
            PricePlan pricePlan = JsonConvert.DeserializeObject<PricePlan>(price);
            if (pricePlan != null)
            {
                itemList.items.Add(new Item()
                {
                    name = "subscription",
                    currency = "USD",
                    price = pricePlan.PriceRange,
                    quantity = "1",
                    sku = "sku"
                });

                var payer = new Payer() { payment_method = "paypal" };

                // Configure Redirect Urls here with RedirectUrls object
                var redirUrls = new RedirectUrls()
                {
                    cancel_url = redirectUrl + "&Cancel=true&LOCALECODE=en_US",
                    return_url = redirectUrl
                };

                // Adding Tax, shipping and Subtotal details
                var details = new Details()
                {
                    tax = "0",
                    shipping = "0",
                    subtotal = pricePlan.PriceRange,
                    shipping_discount = "0"
                };

                //Final amount with details
                var amount = new Amount()
                {
                    currency = "USD",
                    total = (Convert.ToInt32(pricePlan.PriceRange.ToString())).ToString(), // Total must be equal to sum of tax, shipping and subtotal.
                    details = details
                };

                var transactionList = new List<Transaction>();
                transactionList.Add(new Transaction()
                {
                    description = "Subscription with amount of " + pricePlan.PriceRange + " received.",
                    invoice_number = Convert.ToString((new Random()).Next(100000)),
                    amount = amount,
                    item_list = itemList
                });


                payment = new Payment()
                {
                    intent = "sale",
                    payer = payer,
                    transactions = transactionList,
                    redirect_urls = redirUrls
                };

                // Create a payment using a APIContext
                return payment.Create(apiContext);
            }
            else
            {
                return null;
            }
        }

        [HttpPost]
        public ActionResult RegisterCustomerWithBank([FromForm] SignupDto signupDto)
        {
            try
            {
                signupDto.PaymentInfoDto.PaymentType = PaymentType.BankAccount;
                signupDto.userSignupDto.RoleName = "Customer";
                var body = JsonConvert.SerializeObject(signupDto);
                using (HttpClient client = new HttpClient())
                {
                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Get("accessToken"));
                    // Setting Base address.  

                    client.BaseAddress = new Uri(_myConfiguration.RemoteServerUrl+ "api/UserManagement/register");
                    // Setting content type.  
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    // Initialization.  
                    HttpResponseMessage response = new HttpResponseMessage();

                    // HTTP GET  
                    var postTask = client.PostAsJsonAsync<SignupDto>(client.BaseAddress, signupDto);
                    postTask.Wait();
                    var ApiResponse = postTask.Result.Content.ReadAsStringAsync().Result;
                    dynamic value = JsonConvert.DeserializeObject<Object>(ApiResponse);
               
                    // Verification  

                    if (postTask.Result.IsSuccessStatusCode)
                    {
                        return Json(new { success = true, responseText = "Customer Added" });

                    }
                    else if (value== "Email already exists")
                    {
                        return Json(new { success = false, responseText = "Email already exists." });
                    }
                }
                return View();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public IActionResult CheckUserExistance(string email)
        {
            SResponse ress = Fetch.GotoService("api", $"Administrator/CheckUserExistance?email={email}", "GET");
            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                if (ress.Resp == "already exist")
                {
                    return Json("Already exists");
                }
                else
                {
                    return Json("Not Found");
                }
            }
            return Ok();
        }

    }
}
