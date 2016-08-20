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
using Model.BusinessObjects;

namespace MA_WEB.Controllers
{
    [RoutePrefix("api/comments")]
    public class CommentController : BaseController
    {
        private readonly CommentManager _manager = new CommentManager();

        [Route("saveComment")]
        [HttpPost]
        public async Task<int> SaveComment(CommentViewModel comment)
        {
            if (comment == null)
                throw new ArgumentNullException("comment");

            var user = await ValidateCurrentUser();
            if (comment.Id == 0)
            {
                CommentBO commentBo = comment;
                commentBo.Created = DateTime.Now;
                commentBo.CreatedBy = user;
                if (comment.ReqId != 0)
                {
                    commentBo.Requirement = await new RequirementManager().GetRequirement(comment.ReqId);
                }
                else
                {
                    commentBo.Issue = await new IssueManager().GetIssue(comment.IssueId);
                }
                comment = await _manager.Create(commentBo);
                return comment.Id;
            }
            else
            {
                CommentBO commentBo = _manager.GetComment(comment.Id);
                commentBo.Text = comment.Text;
                /*commentBo.Updated = DateTime.Now;
                commentBo.UpdatedBy = user;*/
                await _manager.Update(commentBo);
                return comment.Id;
            }
        }

        [Route("getComments")]
        [Route("getComments/{Id}")]
        [HttpGet]
        public async Task<List<CommentViewModel>> GetComments([FromUri]IdParams parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");

            ValidateParameters(parameters);

            var u = User.Identity.Name;

            var user = await ValidateCurrentUser();

            var comments = _manager.GetComments(parameters.Id);

            var result = new List<CommentViewModel>();
            for (int i = 0; i < comments.Count; i++)
            {
                result.Add(new CommentViewModel()
                {
                    Id = comments[i].Id,
                    Text = comments[i].Text,
                    Date = GetPrettyDate(comments[i].Created),
                    Author = comments[i].CreatedBy.FirstName + " " + comments[i].CreatedBy.LastName,
                    Avatar = comments[i].CreatedBy.Avatar
                });
            }

            return result;
        }

        [Route("getIssueComments")]
        [Route("getIssueComments/{Id}")]
        [HttpGet]
        public async Task<List<CommentViewModel>> GetIssueComments([FromUri]IdParams parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");

            ValidateParameters(parameters);

            var u = User.Identity.Name;

            var user = await ValidateCurrentUser();

            var comments = _manager.GetIssueComments(parameters.Id);

            var result = new List<CommentViewModel>();
            for (int i = 0; i < comments.Count; i++)
            {
                result.Add(new CommentViewModel()
                {
                    Id = comments[i].Id,
                    Text = comments[i].Text,
                    Date = GetPrettyDate(comments[i].Created),
                    Author = comments[i].CreatedBy.FirstName + " " + comments[i].CreatedBy.LastName,
                    Avatar = comments[i].CreatedBy.Avatar
                });
            }

            return result;
        }

        public static string GetPrettyDate(DateTime d)
        {
           
            TimeSpan s = DateTime.Now.Subtract(d);

            int dayDiff = (int)s.TotalDays;

           
            int secDiff = (int)s.TotalSeconds;
            
            if (dayDiff < 0 || dayDiff >= 31)
            {
                return null;
            }

            
            if (dayDiff == 0)
            {
                
                if (secDiff < 60)
                {
                    return "just now";
                }
       
                if (secDiff < 120)
                {
                    return "1 minute ago";
                }

                if (secDiff < 3600)
                {
                    return string.Format("{0} minutes ago",
                        Math.Floor((double)secDiff / 60));
                }
  
                if (secDiff < 7200)
                {
                    return "1 hour ago";
                }

                if (secDiff < 86400)
                {
                    return string.Format("{0} hours ago",
                        Math.Floor((double)secDiff / 3600));
                }
            }

            if (dayDiff == 1)
            {
                return "yesterday";
            }
            if (dayDiff < 7)
            {
                return string.Format("{0} days ago",
                dayDiff);
            }
            if (dayDiff < 31)
            {
                return string.Format("{0} weeks ago",
                Math.Ceiling((double)dayDiff / 7));
            }
            return null;
        }
    }

}
