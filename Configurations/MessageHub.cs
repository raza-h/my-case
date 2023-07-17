using AbsolCase.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static AbsolCase.Configuration.RequestSender;

namespace AbsolCase.Configurations
{
    public class MessageHub : Hub
    {
        private static IHttpContextAccessor httpContextAccessor;
        private static ConcurrentDictionary<string, MessageUser> Users = new ConcurrentDictionary<string, MessageUser>();
        private static IConfiguration config;
        public static void SetHttpContextAccessor(IHttpContextAccessor accessor, IConfiguration configuration)
        {
            httpContextAccessor = accessor;
            config = configuration;
        }

        public override async Task<Task> OnConnectedAsync()
        {
            try
            {
                string userId = GetUserId();
                string connectionId = Context.ConnectionId;

                var user = Users.GetOrAdd(userId, _ => new MessageUser
                {
                    UserId = userId,
                    UserName = GetUserName(),
                    ConnectionIds = new HashSet<string>()
                });

                lock (user.ConnectionIds)
                {
                    user.ConnectionIds.Add(connectionId);
                }
                var users = Users.Select(k => k.Key).ToList();
                string usersJson = JsonConvert.SerializeObject(users);
                await Clients.All.SendAsync("UserConnected", usersJson);
                return base.OnConnectedAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public override async Task<Task> OnDisconnectedAsync(Exception? exception)
        {
            MessageUser messageUser;
            Users.TryRemove(GetUserId(), out messageUser);
            string connectionId = Context.ConnectionId;
            var users = Users.Select(k => k.Key).ToList();
            string usersJson = JsonConvert.SerializeObject(users);
            await Clients.All.SendAsync("UserConnected", usersJson);
            return base.OnDisconnectedAsync(exception);
        }
        public string GetConnectedUsers()
        {
            try
            {
                var users = Users.Select(x => x.Key).ToList();
                return JsonConvert.SerializeObject(users);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // user to user chat
        public void Send(string msg)
        {
            try
            {
                Message message = JsonConvert.DeserializeObject<Message>(msg);
                //add message in database
                message.Id = AddMessageToDatabase(message);
                var body = JsonConvert.SerializeObject(message);
                MessageUser receiver;
                if (Users.TryGetValue(message.ReceiverId, out receiver))
                {
                    foreach (var cid in receiver.ConnectionIds)
                    {
                        Clients.Client(cid).SendAsync("ReceiveMessage", GetUserName(), body);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task SendMessageToAll(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", GetUserName(), message);
        }
        public async Task SendMessage(string sender, string message)
        {
            await Clients.User("187df4cf-4310-4ca3-9af0-a7a778de59a2").SendAsync("ReceiveMessage", message);
        }
        public void ReadMessage(int Id)
        {
            Fetch.GotoService("api", $"Message/ReadMessage?Id={Id}", "PUT");
        }
        public void ReadGroupMessage(string userId, int groupId, int messageId)
        {
            Fetch.GotoService("api", $"Message/ReadGroupMessage?userId={userId}&&groupId={groupId}&&messageId={messageId}", "PUT");
        }

        // for group chat
        public void SendMessageInGroup(string msg)
        {
            try
            {
                Message message = JsonConvert.DeserializeObject<Message>(msg);
                List<GroupUser> groupUsers = GetGroupUsers(message.GroupId.Value);
                var senderUser = groupUsers.Where(x => x.UserId == GetUserId()).FirstOrDefault();
                groupUsers = groupUsers.Where(x => x.UserId != GetUserId()).ToList();
                message.IsGroupMessage = true;
                message.GroupId = message.GroupId;
                message.UserImagePath = senderUser.UserImagePath;
                if (!string.IsNullOrEmpty(message.UserImagePath))
                    message.UserImagePath = $"{config.GetValue<string>("App:RemoteServerUrl")}{message.UserImagePath}";
                else
                    message.UserImagePath = $"{config.GetValue<string>("App:RemoteServerUrl")}Images/blank-profile.png";
                message.Id = AddMessageToDatabase(message);
                var body = JsonConvert.SerializeObject(message);
                MessageUser receiver;
                if (groupUsers != null && groupUsers.Count > 0)
                {
                    foreach (var user in groupUsers)
                    {
                        if (Users.TryGetValue(user.UserId, out receiver))
                        {
                            foreach (var cid in receiver.ConnectionIds)
                            {
                                Clients.Client(cid).SendAsync("ReceiveMessage", GetUserName(), body);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> CreateGroup(string group)
        {
            try
            {
                ChatGroup chatGroup = JsonConvert.DeserializeObject<ChatGroup>(group);
                if (chatGroup != null && chatGroup.UserIds != null && chatGroup.UserIds.Count > 0)
                    chatGroup.UserIds.Add(GetUserId());

                chatGroup.message.MessageText = $"{GetUserName()} created the group {chatGroup.GroupName}";
                chatGroup.message.IsGroupMessage = true;
                chatGroup.message.GroupName = chatGroup.GroupName;
                chatGroup.message.Image = chatGroup.Image;
                int groupId = AddGroupToDatabase(chatGroup);
                chatGroup.message.GroupId = groupId;
                var messageBody = JsonConvert.SerializeObject(chatGroup.message);
                MessageUser receiver;
                if (chatGroup != null && chatGroup.UserIds != null && chatGroup.UserIds.Count > 0)
                {
                    chatGroup.UserIds.Remove(GetUserId());
                    foreach (var userId in chatGroup.UserIds)
                    {
                        if (Users.TryGetValue(userId, out receiver))
                        {
                            foreach (var cid in receiver.ConnectionIds)
                            {
                                await Clients.Client(cid).SendAsync("ReceiveMessage", chatGroup.GroupName, messageBody);
                            }
                        }
                    }
                }
                return groupId;
            }
            catch (Exception ex)

            {
                throw ex;
            }
        }
        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has left the group {groupName}.");
        }
        public async Task<bool> ArchiveChat(string archived, string toDo)
        {
            SResponse resp;
            ArchiveContact archiveContact = JsonConvert.DeserializeObject<ArchiveContact>(archived);
            archiveContact.ContactTwo = archiveContact.ContactOne == GetUserId() ? archiveContact.ContactTwo : archiveContact.ContactOne;
            archiveContact.ContactOne = GetUserId();
            string body = JsonConvert.SerializeObject(archiveContact);
            if (toDo == "archive")
                resp = Fetch.GotoService("api", "Message/AddArchive", "POST", body);
            else
                resp = Fetch.GotoService("api", "Message/UnArchive", "POST", body);
            return resp.Status;
        }

        private string GetUserName()
        {
            try
            {
                string userName = string.Empty;
                string userdto = httpContextAccessor.HttpContext.Session.GetString("userData");
                UserDto userData = JsonConvert.DeserializeObject<UserDto>(userdto);
                if (userData != null)
                    userName = $"{userData.FirstName} "; userName += !string.IsNullOrEmpty(userData.LastName) ? userData.LastName : "";

                return userName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private string GetUserId()
        {
            try
            {
                string userId = string.Empty;
                string userdto = httpContextAccessor.HttpContext.Session.GetString("userData");
                UserDto userData = JsonConvert.DeserializeObject<UserDto>(userdto);
                if (userData != null)
                    userId = userData.Id;

                return userId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private MessageUser GetUser(string username)
        {
            try
            {
                MessageUser user;
                Users.TryGetValue(username, out user);

                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private int AddMessageToDatabase(Message message)
        {
            try
            {
                message.SenderId = GetUserId();
                message.Status = true;
                message.CreatedDate = DateTime.Now;
                var body = JsonConvert.SerializeObject(message);
                SResponse resp = Fetch.GotoService("api", "Message/AddMessage", "POST", body);
                return Convert.ToInt32(resp.Resp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private int AddGroupToDatabase(ChatGroup chatGroup)
        {
            try
            {
                chatGroup.CreatedDate = DateTime.Now;
                chatGroup.CreatedBy = GetUserId();
                chatGroup.AdminId = GetUserId();
                chatGroup.message.Status = true;
                chatGroup.message.CreatedDate = DateTime.Now;
                var body = JsonConvert.SerializeObject(chatGroup);
                SResponse resp = Fetch.GotoService("api", "Message/AddGroup", "POST", body);
                return Convert.ToInt32(resp.Resp);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        private List<GroupUser> GetGroupUsers(int Id)
        {
            try
            {
                List<GroupUser> groupUsers = new List<GroupUser>();
                SResponse resp = Fetch.GotoService("api", $"Message/GetGroupUsers?Id={Id}", "GET");
                if (resp.Status && !string.IsNullOrEmpty(resp.Resp))
                {
                    groupUsers = JsonConvert.DeserializeObject<List<GroupUser>>(resp.Resp);
                }
                return groupUsers;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
