using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using Model.BusinessObjects;

namespace MA_WEB.Models
{
    public class IssueViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ProjectId { get; set; }
        public List<RequirementBO> Requirements { get; set; }
        public  string Created { get; set; }
        public string Updated { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public int AssignedToId { get; set; }
        public string AssignedTo { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
        public string ProjectName { get; set; }
        public static implicit operator IssueBO(IssueViewModel issueView)
        {
            return new IssueBO()
            {
                Id = issueView.Id,
                Name = issueView.Name,
                Description = issueView.Description
            };
        }

        public static implicit operator IssueViewModel(IssueBO requirementBo)
        {
            return new IssueViewModel()
            {
                Id = requirementBo.Id,
                Name = requirementBo.Name,
                Description = requirementBo.Description,
                Created = requirementBo.Created.ToString(),
                CreatedBy = requirementBo.CreatedBy.FirstName + " " + requirementBo.CreatedBy.LastName,
                UpdatedBy = requirementBo.UpdatedBy.FirstName + " " + requirementBo.UpdatedBy.LastName,
                Updated = requirementBo.Updated.ToString(),
                ProjectId = requirementBo.Project?.Id ?? 0,
                AssignedToId = requirementBo.AssignedTo?.Id ?? 0,
                AssignedTo = requirementBo.AssignedTo != null ? requirementBo.AssignedTo.FirstName + " " + requirementBo.AssignedTo.LastName : "",
                Requirements = /*requirementBo.Requirements != null ? (List<RequirementBO>)requirementBo.Requirements :*/ new List<RequirementBO>(),
                Priority = Enum.GetName(typeof(Priority), requirementBo.Priority),
                Status = Enum.GetName(typeof(IssueStatus), requirementBo.Status)
            };
        }
    }
}