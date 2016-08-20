using System;
using System.Collections.Generic;
using Model.Base;

namespace Model.BusinessObjects
{
    public class ProjectBO : BaseDatedBO
    {
         public ICollection<ApplicationUser> Users { get; set; } 
         public ICollection<RequirementBO>  Requirements { get; set; }
         public ICollection<ProjectsRequirenmentAttributeTypesBO> ProjectsRequirenmentAttributeTypes { get; set; }
        public ProjectStatus Status { get; set; }
        public String Version { get; set; }
        public ICollection<DocumentationBO> Docs { get; set; } 
        public ICollection<InvitationBO> Invitations { get; set; } 
    }

    public enum ProjectStatus
    {
        Active = 0,
        Finished = 1,
        Archived = 2
    }
}