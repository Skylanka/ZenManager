using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Model.Base;

namespace Model.BusinessObjects
{
    public class RequirementBO : BaseDatedBO
    {
        public String UniqueTag { get; set; }
        public ICollection<AttributeBO> Attributes { get; set; }
        public ProjectBO Project { get; set; }
        public RequirementsDescriptionBO TypeDescription { get; set; }
        public ICollection<CommentBO> Comments { get; set; } 
    }

    public enum RequirementType
    {
        Uc = 0,
        Supp = 1,
        Term = 2,
        Feat = 3,
        Other = 4
    }
}