using AbsolCase.Configurations;
using AbsolCase.Models;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using Microsoft.Graph.CallRecords;
using Microsoft.Graph.ExternalConnectors;
using Newtonsoft.Json;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static AbsolCase.Configuration.RequestSender;

namespace AbsolCase.Areas.Attorney.Controllers
{
    [ServiceFilter(typeof(ConfigureSession))]
    public class IntegrationsController : Controller
    {
        private readonly IConfiguration configuration;
        public IntegrationsController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        #region Outlook
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult OutlookAccess(string code)
        {
            return View();
        }

        //public IActionResult OutlookAuthentication()
        //{

        //    //clientId= c4e85028-7cf7-4fd3-a075-5e23ebd8ec8d;
        //    //objectId= 02fb17bb-9626-4f65-98eb-fc25cff17073;
        //    //directoryID= 8fd0ffdc-d2ca-47e9-8dde-435212189739;

        //    //https://login.microsoftonline.com/common/oauth2/v2.0/authorize?response_type=code&scope=https://graph.microsoft.com/User.Read&client_id=c4e85028-7cf7-4fd3-a075-5e23ebd8ec8d&redirect_uri=http://localhost:25602/Attorney/OutlookIntegration/OutlookAuthentication

        //    //return Redirect("https://login.microsoftonline.com/organizations/oauth2/v2.0/authorize?response_type=code&scope=https://graph.microsoft.com/User.Read&client_id=c4e85028-7cf7-4fd3-a075-5e23ebd8ec8d&redirect_uri=http://localhost:25602/Attorney/OutlookIntegration/OutlookAuthentication");
        //}
        #endregion




        #region DocuSign
        MyCredential credential = new MyCredential();
        public class MyCredential
        {
            public string UserName
            {
                get;
                set;
            } = "haseeb.shaukat@ab-sol.com";
            public string Password
            {
                get;
                set;
            } = "123456Ha.";
        }
        public IActionResult DocuIndex()
        {
            return View();
        }

        public async Task<IActionResult> Token(string code)
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", $"Integration/GetAuthTokken?code={code}", "Get");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    if (resp.Resp != null)
                    {
                        HttpContext.Session.SetString("docusigntoken", resp.Resp);
                        return RedirectToAction("SendEnvelope");
                    }
                    else
                    {
                        return RedirectToAction("DocuIndex");
                    }
                }
                else
                {
                    //TempData["response"] = resp.Resp;
                    return RedirectToAction("DocuIndex");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> SendEnvelope()
        {

            //string accountId = "aaac805e-f893-411d-bc84-777b1d8ce1ed";
            ////string accessToken = "YOUR_ACCESS_TOKEN";
            ////https://demo.docusign.net/restapi//v2.1/accounts/aaac805e-f893-411d-bc84-777b1d8ce1ed/envelopes
            //string apiUrll = "https://demo.docusign.net/restapi//v2.1"; // Use the appropriate base URL for your desired environment

            //HttpClient client = new HttpClient();
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //string requestUrl = $"{apiUrll}/accounts/{accountId}/envelopes";

            //var request = new HttpRequestMessage(HttpMethod.Post, requestUrl);

            //// Set the request body with the envelope data
            //var envelopeData = new
            //{
            //    emailSubject = "Your envelope subject",
            //    emailBlurb = "Your envelope message",
            //    status = "sent",
            //    recipients = new
            //    {
            //        signers = new[]
            //        {
            //                new
            //                {
            //                    email = "recipient@example.com",
            //                    name = "Recipient Name",
            //                    recipientId = "1",
            //                    tabs = new
            //                    {
            //                        signHereTabs = new[]
            //                        {
            //                            new
            //                            {
            //                                anchorString = "Please Sign Here",
            //                                anchorUnits = "pixels",
            //                                anchorXOffset = "10",
            //                                anchorYOffset = "10"
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //    },
            //    documents = new[]
            //    {
            //            new
            //            {
            //                documentId = "1",
            //                name = "Document Name",
            //                fileExtension = "txt",
            //                documentBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes("Document content"))
            //            }
            //        }
            //};

            //string jsonPayload = JsonConvert.SerializeObject(envelopeData);
            //request.Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            //HttpResponseMessage responseee = await client.SendAsync(request);

            //if (responseee.IsSuccessStatusCode)
            //{
            //    // Envelope sent successfully
            //    string responseContentttttt = await responseee.Content.ReadAsStringAsync();
            //    // Process the response content
            //}
            //else
            //{
            //    // Handle the API request error
            //    string errorMessage = await responseee.Content.ReadAsStringAsync();
            //    // Handle the error message
            //}
            //return RedirectToAction("DocuIndex");
            return View();
        }

        [HttpPost]
        public IActionResult SendEnvelopePost(DocumentSign sign)
        {
            try
            {
                string gettoken = "";
                if (HttpContext.Session.GetString("docusigntoken") != null)
                {
                    gettoken = HttpContext.Session.GetString("docusigntoken");
                    var responseObject = JsonConvert.DeserializeObject<dynamic>(gettoken);
                    // Retrieve the token from the response
                    string token = responseObject.access_token;
                    sign.AccessToken = token;
                }
                if (sign.Document != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        sign.Document.CopyTo(ms);
                        byte[] fileBytes = ms.ToArray();
                        string base64String = Convert.ToBase64String(fileBytes);
                        sign.DocumentString = base64String;
                        sign.DocumentName = sign.Document.FileName;
                        sign.Document = null;
                    }
                }

                var body = JsonConvert.SerializeObject(sign);
                SResponse resp = Fetch.GotoService("api", $"Integration/SendEnvelope", "Post", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    if (resp.Resp != null)
                    {
                        dynamic deserializedObject = JsonConvert.DeserializeObject<dynamic>(resp.Resp);
                        string data = (string)deserializedObject.message;
                        if (data == "Document Sent")
                        {
                            return Json("Document Sent");
                        }
                        else
                        {
                            return Json("Document Not Sent");
                        }
                    }
                    else
                    {
                        return Json("error");
                    }
                }
                else
                {
                    //TempData["response"] = resp.Resp;
                    return Json("error");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IActionResult ManageDocumentSign()
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", $"Integration/GetAllDocumentSign", "Get");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    var sa= HttpContext.Session.GetString("docusigntoken");
                    List<DocumentSign> _resultModel = JsonConvert.DeserializeObject<List<DocumentSign>>(resp.Resp);
                    return View(_resultModel);
                }
                if (resp.Resp == "Please Add Firm Details First")
                {
                    TempData["response"] = "Please Add Firm Details First";
                    return RedirectToAction("AddFirm", "Admin", new { area = "Attorney" });
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

        public async Task<IActionResult> GetDocumentFromDocuSignApi(string envelopeId,string status)
        {
            try
            {
                if (status == "completed")
                {
                    return RedirectToAction("DownloadFiles", new { id = envelopeId });
                }
                string accessToken = "";
                string gettoken = "";
                if (HttpContext.Session.GetString("docusigntoken") != null)
                {
                    gettoken = HttpContext.Session.GetString("docusigntoken");
                    var responseObject = JsonConvert.DeserializeObject<dynamic>(gettoken);
                    string token = responseObject.access_token;
                    accessToken = token;
                    //SResponse resp = Fetch.GotoService("api", $"Integration/GetDocumentFromDocuSignApi", "Get");
                    SResponse resp = Fetch.GotoService("api", $"Integration/GetEnvelopeDocs?envelopeId={envelopeId}&&accessToken={accessToken}", "Get");
                    if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                    {
                        return RedirectToAction("DownloadFiles", new { id = resp.Resp });

                    }
                    if (resp.Resp == "Please Add Firm Details First")
                    {
                        TempData["response"] = "Please Add Firm Details First";
                        return RedirectToAction("AddFirm", "Admin", new { area = "Attorney" });
                    }
                    else
                    {
                        TempData["response"] = resp.Resp;
                        return RedirectToAction("ManageDocumentSign");
                    }
                }
                else
                {
                    return RedirectToAction("DocuIndex");
                }

            }
            catch (Exception)
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
                //int docid = Convert.ToInt32(id);
                string _decumentPath = string.Empty;
                SResponse resp = AbsolCase.Configuration.RequestSender.Fetch.GotoService("api", $"Integration/GetDocumentsByEnvelopeId?Id={id}", "GET");
                DocumentSign DecumentResp = JsonConvert.DeserializeObject<DocumentSign>(resp.Resp);
                fileExtention = DecumentResp.DocumentSavedPath.Substring(DecumentResp.DocumentSavedPath.IndexOf('.') + 1);
                path = $"{configuration.GetValue<string>("App:RemoteServerUrl")}" + $"{DecumentResp.DocumentSavedPath}";
                WebClient wc = new WebClient();
                //string path = fileName;
                //Read the File data into Byte Array.
                byte[] bytes = wc.DownloadData(path);
                string Filename = DecumentResp.DocumentName;
                //string Filename = DecumentResp.DocumentName + "." + fileExtention;

                return File(bytes, "application/x-msdownload", Filename);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        #endregion
    }
}
