using AbsolCase.Configurations;
using AbsolCase.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using static AbsolCase.Configuration.RequestSender;

namespace AbsolCase.Areas.Attorney.Controllers
{
    [ServiceFilter(typeof(ConfigureSession))]
    public class MessageController : Controller
    {
        private readonly IConfiguration config;

        public MessageController(IConfiguration config)
        {
            this.config = config;
        }
        public IActionResult Message()
        {
            try
            {
                string userId = string.Empty;
                string userdto = HttpContext.Session.GetString("userData");
                UserDto userData = JsonConvert.DeserializeObject<UserDto>(userdto);
                if (userData != null)
                    userId = userData.Id;

                SResponse resp = Fetch.GotoService("api", $"Message/GetMessagesByUserId?userId={userId}", "GET");
                if (resp.ErrorCode == 401)
                    return RedirectToAction("Login", "Account", new { area = "Security" });

                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    List<Message> messages = JsonConvert.DeserializeObject<List<Message>>(resp.Resp);
                    if (messages != null && messages.Count > 0)
                    {
                        foreach (var message in messages)
                        {
                            if (!string.IsNullOrEmpty(message.UserImagePath) && !string.IsNullOrEmpty(message.UserImagePath.Trim()))
                                message.UserImagePath = $"{config.GetValue<string>("App:RemoteServerUrl")}{message.UserImagePath}";
                            else
                                message.UserImagePath = $"{config.GetValue<string>("App:RemoteServerUrl")}Images/blank-profile.png";
                        }
                    }
                    return View(messages);
                }
                else
                    return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IActionResult LoadChatMessages(string senderId, string receiverId)
        {
            try
            {
                string userId = string.Empty;
                string userdto = HttpContext.Session.GetString("userData");
                UserDto userData = JsonConvert.DeserializeObject<UserDto>(userdto);
                if (userData != null)
                    userId = userData.Id;

                SResponse resp = Fetch.GotoService("api", $"Message/GetMessagesBySenderIdAndReceiverId?senderId={senderId}&&receiverId={receiverId}&&userId={userId}", "GET");
                if (resp.ErrorCode == 401)
                    return RedirectToAction("Login", "Account", new { area = "Security" });

                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    List<Message> messages = JsonConvert.DeserializeObject<List<Message>>(resp.Resp);
                    if (messages != null && messages.Count > 0)
                    {
                        foreach (var message in messages)
                        {
                            if (!string.IsNullOrEmpty(message.ImagePath) && !string.IsNullOrEmpty(message.ImagePath.Trim()))
                                message.ImagePath = $"{config.GetValue<string>("App:RemoteServerUrl")}{message.ImagePath}";
                            if (!string.IsNullOrEmpty(message.UserImagePath))
                                message.UserImagePath = $"{config.GetValue<string>("App:RemoteServerUrl")}{message.UserImagePath}";
                            else
                                message.UserImagePath = $"{config.GetValue<string>("App:RemoteServerUrl")}Images/blank-profile.png";
                        }
                    }
                    return PartialView("~/Views/PartialViews/_Chat.cshtml", messages);
                }
                else
                    return Ok();
            }
            catch (Exception ex)
            {
                return Ok();
            }
        }

        public IActionResult LoadGroupMessages(int groupId)
        {
            try
            {
                string userId = string.Empty;
                string userdto = HttpContext.Session.GetString("userData");
                UserDto userData = JsonConvert.DeserializeObject<UserDto>(userdto);
                if (userData != null)
                    userId = userData.Id;

                SResponse resp = Fetch.GotoService("api", $"Message/GetGroupMessages?groupId={groupId}&&userId={userId}", "GET");
                if (resp.ErrorCode == 401)
                    return RedirectToAction("Login", "Account", new { area = "Security" });

                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    List<Message> messages = JsonConvert.DeserializeObject<List<Message>>(resp.Resp);
                    if (messages != null && messages.Count > 0)
                    {
                        foreach (var message in messages)
                        {
                            if (!string.IsNullOrEmpty(message.ImagePath) && !string.IsNullOrEmpty(message.ImagePath.Trim()))
                                message.ImagePath = $"{config.GetValue<string>("App:RemoteServerUrl")}{message.ImagePath}";
                            if (!string.IsNullOrEmpty(message.UserImagePath) && !string.IsNullOrEmpty(message.UserImagePath.Trim()))
                                message.UserImagePath = $"{config.GetValue<string>("App:RemoteServerUrl")}{message.UserImagePath}";
                            else
                                message.UserImagePath = $"{config.GetValue<string>("App:RemoteServerUrl")}Images/blank-profile.png";
                        }
                    }
                    return PartialView("~/Views/PartialViews/_Chat.cshtml", messages);
                }
                else
                    return Ok();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
