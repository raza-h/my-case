using AbsolCase.Configurations;
using AbsolCase.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using static AbsolCase.Configuration.RequestSender;

namespace AbsolCase.Areas.Administration.Controllers
{
    [ServiceFilter(typeof(ConfigureSession))]
    public class ConfigurationController : Controller
    {
        #region contact group
        public IActionResult AddContactGroup()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddContactGroup(ContactGroup contactGroup)
        {
            try
            {
                var body = JsonConvert.SerializeObject(contactGroup);
                SResponse resp = Fetch.GotoService("api", "ConfigManagement/AddContactGroup", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ManageContactGroup");
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
        public IActionResult ManageContactGroup()
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", "ConfigManagement/GetContactGroups", "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    List<ContactGroup> contactGroups = JsonConvert.DeserializeObject<List<ContactGroup>>(resp.Resp);
                    return View(contactGroups);
                }
                else
                    return View();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public IActionResult UpdateContactGroup(int Id)
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", $"ConfigManagement/GetContactGroupById?Id={Id}", "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    ContactGroup contactGroups = JsonConvert.DeserializeObject<ContactGroup>(resp.Resp);
                    return View(contactGroups);
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
        public IActionResult UpdateContactGroup(ContactGroup contactGroup)
        {
            try
            {
                var body = JsonConvert.SerializeObject(contactGroup);
                SResponse resp = Fetch.GotoService("api", "ConfigManagement/UpdateContactGroup", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ManageContactGroup");
                }
                else
                {
                    TempData["response"] = resp.Resp;
                    return View();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public IActionResult DeleteContactGroup(int Id)
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", $"ConfigManagement/DeleteContactGroup?Id={Id}", "DELETE");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ManageContactGroup");
                }
                return RedirectToAction("ManageContactGroup");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region practice area
        public IActionResult AddPracticeArea()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddPracticeArea(PracticeArea contactGroup)
        {
            try
            {
                var body = JsonConvert.SerializeObject(contactGroup);
                SResponse resp = Fetch.GotoService("api", "ConfigManagement/AddPracticeArea", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ManagePracticeArea");
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
        public IActionResult ManagePracticeArea()
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", "ConfigManagement/GetPracticeAreas", "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    List<PracticeArea> practiceArea = JsonConvert.DeserializeObject<List<PracticeArea>>(resp.Resp);
                    return View(practiceArea);
                }
                else
                    return View();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public IActionResult UpdatePracticeArea(int Id)
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", $"ConfigManagement/GetPracticeAreaById?Id={Id}", "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    PracticeArea practiceArea = JsonConvert.DeserializeObject<PracticeArea>(resp.Resp);
                    return View(practiceArea);
                }
                else
                    return View();
            }
            catch(Exception ex)
            {
                throw;
            }
        }
        [HttpPost]
        public IActionResult UpdatePracticeArea(PracticeArea practiceArea)
        {
            try
            {
                var body = JsonConvert.SerializeObject(practiceArea);
                SResponse resp = Fetch.GotoService("api", "ConfigManagement/UpdatePracticeArea", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ManagePracticeArea");
                }
                else
                {
                    TempData["response"] = resp.Resp;
                    return View();
                }
            }
            catch(Exception ex)
            {
                throw;
            }
        }
        public IActionResult DeletePracticeArea(int Id)
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", $"ConfigManagement/DeletePracticeArea?Id={Id}", "DELETE");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ManagePracticeArea");
                }
                return RedirectToAction("ManagePracticeArea");
            }
            catch(Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region user title
        public IActionResult AddUserTitle()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddUserTitle(UserTitle userTitle)
        {
            try
            {
                var body = JsonConvert.SerializeObject(userTitle);
                SResponse resp = Fetch.GotoService("api", "ConfigManagement/AddUserTitle", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ManageUserTitle");
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
        public IActionResult ManageUserTitle()
        {
            try
            {
                SResponse resp = AbsolCase.Configuration.RequestSender.Fetch.GotoService("api", "ConfigManagement/GetUserTitles", "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    List<UserTitle> userTitle = JsonConvert.DeserializeObject<List<UserTitle>>(resp.Resp);
                    return View(userTitle);
                }
                else
                    return View();
            }
            catch(Exception ex)
            {
                throw;
            }
        }
        public IActionResult UpdateUserTitle(int Id)
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", $"ConfigManagement/GetUserTitleById?Id={Id}", "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    UserTitle userTitle = JsonConvert.DeserializeObject<UserTitle>(resp.Resp);
                    return View(userTitle);
                }
                else
                    return View();
            }
            catch(Exception ex)
            {
                throw;
            }
        }
        [HttpPost]
        public IActionResult UpdateUserTitle(UserTitle userTitle)
        {
            try
            {
                var body = JsonConvert.SerializeObject(userTitle);
                SResponse resp = Fetch.GotoService("api", "ConfigManagement/UpdateUserTitle", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ManageUserTitle");
                }
                else
                {
                    TempData["response"] = resp.Resp;
                    return View();
                }
            }
            catch(Exception ex)
            {
                throw;
            }
        }
        public IActionResult DeleteUserTitle(int Id)
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", $"ConfigManagement/DeleteUserTitle?Id={Id}", "DELETE");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ManageUserTitle");
                }
                return RedirectToAction("ManageUserTitle");
            }
            catch(Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region refferal source
        public IActionResult AddRefferalSource()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddRefferalSource(RefferalSource refferalSource)
        {
            try
            {
                var body = JsonConvert.SerializeObject(refferalSource);
                SResponse resp = Fetch.GotoService("api", "ConfigManagement/AddRefferalSource", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ManageRefferalSource");
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
        public IActionResult ManageRefferalSource()
        {
            try
            {
                SResponse resp = AbsolCase.Configuration.RequestSender.Fetch.GotoService("api", "ConfigManagement/GetRefferalSources", "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    List<RefferalSource> refferalSource = JsonConvert.DeserializeObject<List<RefferalSource>>(resp.Resp);
                    return View(refferalSource);
                }
                else
                    return View();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public IActionResult UpdateRefferalSource(int Id)
        {
            try
            {
                SResponse resp = AbsolCase.Configuration.RequestSender.Fetch.GotoService("api", $"ConfigManagement/GetRefferalSourceById?Id={Id}", "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    RefferalSource refferalSource = JsonConvert.DeserializeObject<RefferalSource>(resp.Resp);
                    return View(refferalSource);
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
        public IActionResult UpdateRefferalSource(RefferalSource refferalSource)
        {
            try
            {
                var body = JsonConvert.SerializeObject(refferalSource);
                SResponse resp = Fetch.GotoService("api", "ConfigManagement/UpdateRefferalSource", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ManageRefferalSource");
                }
                else
                {
                    TempData["response"] = resp.Resp + " " + "Unable To update Refferal Source";
                    return View();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public IActionResult DeleteRefferalSource(int Id)
        {
            try
            {
                SResponse resp = AbsolCase.Configuration.RequestSender.Fetch.GotoService("api", $"ConfigManagement/DeleteRefferalSource?Id={Id}", "DELETE");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ManageRefferalSource");
                }
                return RedirectToAction("ManageRefferalSource");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Billing Method
        public IActionResult AddBillingMethod()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddBillingMethod(BillingMethod billingMethod)
        {
            try
            {
                var body = JsonConvert.SerializeObject(billingMethod);
                SResponse resp = Fetch.GotoService("api", "ConfigManagement/AddBillingMethod", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ManageBillingMethod");
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
        public IActionResult ManageBillingMethod()
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", "ConfigManagement/GetBillingMethods", "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    List<BillingMethod> billingMethod = JsonConvert.DeserializeObject<List<BillingMethod>>(resp.Resp);
                    return View(billingMethod);
                }
                else
                    return View();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public IActionResult UpdateBillingMethod(int Id)
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", $"ConfigManagement/GetBillingMethodById?Id={Id}", "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    BillingMethod billingMethod = JsonConvert.DeserializeObject<BillingMethod>(resp.Resp);
                    return View(billingMethod);
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
        public IActionResult UpdateBillingMethod(BillingMethod billingMethod)
        {
            try
            {
                var body = JsonConvert.SerializeObject(billingMethod);
                SResponse resp = Fetch.GotoService("api", "ConfigManagement/UpdateBillingMethod", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ManageBillingMethod");
                }
                else
                {
                    TempData["response"] = resp.Resp;
                    return View();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public IActionResult DeleteBillingMethod(int Id)
        {
            try
            {
                SResponse resp = AbsolCase.Configuration.RequestSender.Fetch.GotoService("api", $"ConfigManagement/DeleteBillingMethod?Id={Id}", "DELETE");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ManageBillingMethod");
                }
                return RedirectToAction("ManageBillingMethod");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion


        #region Notes Tag
        public IActionResult AddAdminNotesTag()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddAdminNotesTag(NotesTag model)
        {
            try
            {
                var body = JsonConvert.SerializeObject(model);
                SResponse resp = Fetch.GotoService("api", "Notes/AddAdminNotesTag", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {

                    return RedirectToAction("ManageAdminNotesTag");
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
        public IActionResult ManageAdminNotesTag()
        {
            try
            {
                string userId = string.Empty;
                string userdto = HttpContext.Session.GetString("userData");
                if (!string.IsNullOrEmpty(userdto))
                {
                    UserDto userData = JsonConvert.DeserializeObject<UserDto>(userdto);
                    userId = userData.Id;
                }

                SResponse resp = Fetch.GotoService("api", "Notes/GetAdminNotesTag?userId={userId}", "GET");

                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    List<NotesTag> _resultModel = JsonConvert.DeserializeObject<List<NotesTag>>(resp.Resp);
                    return View(_resultModel);
                }

                else
                {
                    TempData["response"] = resp.Resp;
                    return View();
                }
                return View();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public IActionResult UpdateAdminNotesTag(int Id)
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", $"Notes/GetAdminNotesTagById?Id={Id}", "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    NotesTag _notesTag = JsonConvert.DeserializeObject<NotesTag>(resp.Resp);
                    return View(_notesTag);
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
        public IActionResult UpdateAdminNotesTag(NotesTag model)
        {
            try
            {

                var body = JsonConvert.SerializeObject(model);
                SResponse resp = Fetch.GotoService("api", "Notes/UpdateAdminNotesTag", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ManageAdminNotesTag");
                }
                else
                {
                    TempData["response"] = resp.Resp;
                    return View();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }





        #endregion

        #region Document
        public IActionResult AddAdminDocumentTag()
        {
            string userId = string.Empty;
            string userdto = HttpContext.Session.GetString("userData");
            if (!string.IsNullOrEmpty(userdto))
            {
                UserDto userData = JsonConvert.DeserializeObject<UserDto>(userdto);
                userId = userData.Id;
            }
            SResponse resp = Fetch.GotoService("api", $"Firm/GetFirmByUserId?userId={userId}", "GET");
            if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
            {
                return View();
            }
            else
            {
                if (resp.Resp == "Please Add Firm Details First")
                {
                    TempData["response"] = "Please Add Firm Details First";
                    return RedirectToAction("AddFirm", "Firm", new { area = "Attorney" });
                }
                else
                {
                    TempData["response"] = resp.Resp;
                    return View();
                }
            }
            return View();
        }
        [HttpPost]
        public IActionResult AddAdminDocumentTag(DocumentTag model)
        {
            try
            {
                var body = JsonConvert.SerializeObject(model);
                SResponse resp = Fetch.GotoService("api", "ConfigManagement/AddAdminDocumentTag", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ManageAdminDocumentTag");
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
        public IActionResult ManageAdminDocumentTag()
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", "ConfigManagement/GetAdminDocumentTag", "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    List<DocumentTag> _resultModel = JsonConvert.DeserializeObject<List<DocumentTag>>(resp.Resp);
                    return View(_resultModel);
                }
               
                else
                {
                    TempData["response"] = resp.Resp;
                    return View();
                }
              
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public IActionResult UpdateAdminDocumentTag(int Id)
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", $"ConfigManagement/GetAdminDocumentTagById?Id={Id}", "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    DocumentTag _documentTag = JsonConvert.DeserializeObject<DocumentTag>(resp.Resp);
                    return View(_documentTag);
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
        public IActionResult UpdateAdminDocumentTag(DocumentTag model)
        {
            try
            {
                var body = JsonConvert.SerializeObject(model);
                SResponse resp = AbsolCase.Configuration.RequestSender.Fetch.GotoService("api", "ConfigManagement/UpdateAdminDocumentTag", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ManageAdminDocumentTag");
                }
                else
                {
                    TempData["response"] = resp.Resp;
                    return View();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public IActionResult DeleteAdminDocumentTag(int Id)
        {
            try
            {
                SResponse resp = Fetch.GotoService("api", $"ConfigManagement/DeleteAdminDocumentTag?Id={Id}", "DELETE");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return RedirectToAction("ManageDocumentTag");
                }
                return RedirectToAction("ManageAdminDocumentTag");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion
    }
}
