using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Model.Base;
using Model.BusinessObjects;
using WebGrease.Css.Extensions;

namespace MA_WEB.Models
{
    public class ProjectViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string Created { get; set; }
        public string Updated { get; set; }
        public string Author { get; set; }
        public List<UserProject> Participants { get; set; } 
        public string Status { get; set; }
        public string Version { get; set; }
        public int IssueCount { get; set; }
        public int RequirementCount { get; set; }
        public List<String> Docs { get; set; } 

        public static implicit operator ProjectBO(ProjectViewModel project)
        {
            return new ProjectBO() { Id = project.Id, Name = project.Name, Description = project.Description, Users = new Collection<ApplicationUser>(), Updated = DateTime.Now, Version = project.Version};
        }

        public static implicit operator ProjectViewModel(ProjectBO project)
        {
            List<UserProject> part = new List<UserProject>();
            foreach (var u in project.Users)
            {

                part.Add(new UserProject()
                {
                    Id = u.Id,
                    Name = u.FirstName + " " + u.LastName,
                    Email =  u.Email,
                    Phone = u.Phone
                });
            }
            List<String> docs = new List<string>();
            foreach (var u in project.Docs)
            {
                docs.Add(u.Name);
            }

            return new ProjectViewModel()
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                Author = project.CreatedBy.FirstName + " " + project.CreatedBy.LastName,
                Created = project.Created.ToString(),
                Participants = part,
                Updated = project.Updated.ToString(),
                Version = project.Version,
                Status = Enum.GetName(typeof(ProjectStatus), project.Status),
                Docs = docs
            };
        }
    }

    public class UserProject
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
