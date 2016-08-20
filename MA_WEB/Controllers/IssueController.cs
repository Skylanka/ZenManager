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
    [RoutePrefix("api/issues")]
    public class IssueController : BaseController
    {
        private readonly  IssueManager _manager = new IssueManager();

        [Route("saveIssue")]
        [HttpPost]
        public async Task<int> SaveIssue(IssueViewModel issue)
        {
            if(issue == null)
                throw new ArgumentNullException("issue");

            var user = await ValidateCurrentUser();

            ProjectBO project = await new ProjectManager().GetProject(issue.ProjectId);
            var assignedTo = await ValidateCurrentUser(issue.AssignedToId);

            if (issue.Id == 0)
            {
                IssueBO issueBo = issue;
                issueBo.Created = DateTime.Now;
                issueBo.Updated = DateTime.Now;
                issueBo.UpdatedBy = user;
                issueBo.CreatedBy = user;
                issueBo.Project = project;
                issueBo.AssignedTo = assignedTo;
                issueBo.Priority = (Priority)Enum.Parse(typeof(Priority), issue.Priority);
                issueBo.Status = (IssueStatus)Enum.Parse(typeof(IssueStatus), issue.Status);
                issue = await _manager.Create(issueBo);
                return issue.Id;
            }
            else
            {
                IssueBO issueBo = await _manager.GetIssue(issue.Id);
                issueBo.Updated = DateTime.Now;
                issueBo.UpdatedBy = user;
                issueBo.Priority = (Priority)Enum.Parse(typeof(Priority), issue.Priority);
                issueBo.Status = (IssueStatus)Enum.Parse(typeof(IssueStatus), issue.Status);
                issueBo.Description = issue.Description;
                issueBo.Name = issue.Name;
                await _manager.Update(issueBo);
                return issue.Id;
            }
        }

        
        [Route("addIssueRequirementLink")]
        [HttpPost]
        public async Task AddIssueRequirementLink(IssueLinkViewModel link)
        {
            if (link == null)
                throw new ArgumentNullException("link");

            RequirementBO requirement = await new RequirementManager().GetRequirement(link.RequirementId);
            IssueBO issue = await _manager.GetIssue(link.IssueId);
            issue.Requirements.Add(requirement);
            await _manager.Update(issue);
        }

        [Route("getIssue")]
        [Route("getIssue/{Id}")]
        [HttpGet]
        public async Task<IssueViewModel> GetIssue([FromUri]IdParams parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");

            ValidateParameters(parameters);
            
            var project =  await _manager.GetIssue(parameters.Id);
            return project;
        }


        [Route("getIssueLinks")]
        [Route("getIssueLinks/{Id}")]
        [HttpGet]
        public async Task<List<RequirementLinkViewModel>> GetIssueLinks([FromUri] IdParams parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");

            ValidateParameters(parameters);

           /* ICollection<RequirementBO> linksCollection = await _manager.GetRequirementLinks(parameters.Id);*/
            var issue =  await _manager.GetRequirementLinks(parameters.Id);
            List<RequirementBO> links = issue.Requirements.ToList();
            List<RequirementLinkViewModel> result = new List<RequirementLinkViewModel>();
            for (int i = 0; i < links.Count; i++)
            {
                result.Add(new RequirementLinkViewModel()
                {
                    Name = links[i].Name,
                    ReqId = links[i].Id,
                    SpecId = links[i].TypeDescription.Id,
                    Specification = links[i].TypeDescription.Name,
                    Tag = links[i].UniqueTag
                });
            }
            return result;
        }

        [Route("getIssues")]
        [Route("getIssues/{Id}")]
        [HttpGet]
        public async Task<List<IssueViewModel>> GetIssues([FromUri]IdParams parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");

            ValidateParameters(parameters);

            var u = User.Identity.Name;

            var user = await ValidateCurrentUser();

            var issues =  _manager.GetIssues(parameters.Id);

            var result = new List<IssueViewModel>();
            for (int i = 0; i < issues.Count; i++)
            {
/*
                if (issues[i].Status == IssueStatus.Assigned)
                {

                }*/

                result.Add(new IssueViewModel()
                {
                    Id = issues[i].Id,
                    Name = issues[i].Name,
                    Description = issues[i].Description,
                    Created = issues[i].Created.ToString(),
                    CreatedBy = issues[i].CreatedBy.FirstName + ' ' + issues[i].CreatedBy.LastName,
                    Updated = issues[i].Updated.ToString(),
                    UpdatedBy = issues[i].UpdatedBy.FirstName + ' ' + issues[i].UpdatedBy.LastName,
                    AssignedTo = issues[i].AssignedTo.FirstName + ' ' + issues[i].AssignedTo.LastName,
                    AssignedToId = issues[i].AssignedTo.Id,
                    ProjectId = issues[i].Project.Id,
                    Priority = Enum.GetName(typeof(Priority), issues[i].Priority),
                    Status = Enum.GetName(typeof(IssueStatus), issues[i].Status)
                });
            }

            return result;
        }

        [Route("getLatestIssues")]
        [HttpGet]
        public async Task<List<IssueViewModel>> GetLatestIssues()
        {

            var u = User.Identity.Name;

            var user = await ValidateCurrentUser();

            var issues = _manager.GetIssues(user);

            var result = new List<IssueViewModel>();
            for (int i = 0; i < issues.Count; i++)
            {
                result.Add(new IssueViewModel()
                {
                    Id = issues[i].Id,
                    Name = issues[i].Name,
                    Created = issues[i].Created.ToString(),
                    CreatedBy = issues[i].CreatedBy.FirstName + ' ' + issues[i].CreatedBy.LastName,
                    AssignedTo = issues[i].AssignedTo.FirstName + ' ' + issues[i].AssignedTo.LastName,
                    ProjectId = issues[i].Project.Id,
                    Priority = Enum.GetName(typeof(Priority), issues[i].Priority),
                    Status = Enum.GetName(typeof(IssueStatus), issues[i].Status),
                    ProjectName = issues[i].Project.Name
                });
            }

            return result;
        }


        [Route("getUsersForProject")]
        [HttpGet]
        public List<UserViewModel> GetUsersForProject(int id)
        {
            return new List<UserViewModel>();
        }


        // [Authorize(Roles = "Admin, CompanyAdmin, Engineer")]
        [Route("getIssuesCount")]
        [Route("getIssuesCount/{Id}")]
        [HttpGet]
        public async Task<int> GetIssuesCount([FromUri]IdParams parameters)
        {
            ValidateParameters(parameters);
            var c = await _manager.GetCount(parameters.Id);
            return c;
        }

        [Route("deleteIssue")]
        [HttpPost]
        public async Task DeleteIssue(IdParams parameters)
        {
            ValidateParameters(parameters);

            var user = await ValidateCurrentUser();
            await _manager.Delete(parameters.Id);
        }
    }
}
