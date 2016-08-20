using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using Model.BusinessObjects;

namespace MA_WEB.Models
{
    public class RequirementViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int RequirementType { get; set; }
        public string Specification { get; set; }
        public string UniqueTag { get; set; }
        public List<AttributeBO> Attr { get; set; }
        public ICollection<Attributes> Attributes { get; set; }
        public  string Created { get; set; }
        public string Updated { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }

        public static implicit operator RequirementBO(RequirementViewModel requirementView)
        {
            return new RequirementBO()
            {
                Id = requirementView.Id,
                Name = requirementView.Name,
                Description = requirementView.Description,
                Attributes = new Collection<AttributeBO>(),
                UniqueTag = requirementView.UniqueTag
            };
        }

        public static implicit operator RequirementViewModel(RequirementBO requirementBo)
        {
            return new RequirementViewModel()
            {
                Id = requirementBo.Id,
                Name = requirementBo.Name,
               // Description = requirementBo.Description,
                UniqueTag = requirementBo.UniqueTag,
                Created = requirementBo.Created.ToString(),
/*                CreatedBy = requirementBo.Author.FirstName + " " + requirementBo.Author.LastName,
                UpdatedBy = requirementBo.UpdatedBy.FirstName + " " + requirementBo.UpdatedBy.LastName,
                Updated = requirementBo.Updated.ToString(),*/
                Specification = requirementBo.TypeDescription.Name,
                RequirementType = requirementBo.TypeDescription.Id,
                ProjectId = requirementBo.Project.Id,
                ProjectName = requirementBo.Project.Name
            };
        }
    }
}