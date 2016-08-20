using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using MA_WEB.Controllers.Parameters;
using MA_WEB.Managers;
using MA_WEB.Models;
using Microsoft.AspNet.Identity;
using Model.BusinessObjects;

namespace MA_WEB.Controllers
{
    [RoutePrefix("api/chat")]
    public class ChatController : BaseController
    {
        private readonly ChatManager _chatManager = new ChatManager();

        [Route("getChat")]
        [Route("getChat/{Id}/{Page}/{PageSize}")]
        [HttpGet]
        public async Task<IEnumerable<ChatRecordViewModel>> GetChat([FromUri]IdPagingParams parameters)
        {
            ValidateParameters(parameters);
            var user = await ValidateCurrentUser();

            var projectManager = new ProjectManager();
            ProjectBO project = await projectManager.GetProject(parameters.Id, user);
            if (project == null)
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Project doesn't exist"));

            List<ChatRecordBO> bos = await _chatManager.Get(project, parameters.Page, parameters.PageSize);

            var models = new List<ChatRecordViewModel>();
            foreach (var chatRecord in bos)
            {
                models.Add(new ChatRecordViewModel(chatRecord));
            }

            // returns correct sorting after mapping
            models.Reverse();

            return models;
        }
    }
}
