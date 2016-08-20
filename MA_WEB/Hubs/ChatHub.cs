using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using MA_WEB.Managers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Model.Base;
using Model.BusinessObjects;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR;

namespace MA_WEB.Hubs
{
    [HubName("chatHub")]
    public class ChatHub : Hub
    {
        public Task Connect(int projectId)
        {
            return Groups.Add(Context.ConnectionId, projectId.ToString());
        }

        public Task Disconnect(int projectId)
        {
            return Groups.Remove(Context.ConnectionId, projectId.ToString());
        }

        public async Task Send(int projectId, string message)
        {
            var c = Context;
            ApplicationUser user = new UserManager().Get(c.User.Identity.Name);
            HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();

            if (user == null)
            {
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            };

            var projectManager = new ProjectManager();
            ProjectBO project = await projectManager.GetProject(projectId, user);

            if (project == null)
            {
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }

            var chatManager = new ChatManager();
            var chatRecord = new ChatRecordBO()
            {
                Message = message,
                User = user,
                Time = DateTime.Now,
                Project = project
            };

            chatManager.Add(chatRecord);

            // Call the addNewMessageToPage method to update clients.
            Clients.Group(projectId.ToString()).addNewMessageToPage(user.FirstName + " " + user.LastName, message, chatRecord.Time.ToString("g"), user.Avatar, user.UserName);
        }
    }
}