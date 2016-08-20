using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model.BusinessObjects;

namespace MA_WEB.Models
{
    public class RequirementTypeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Count { get; set; }
        public string Tag { get; set; }
        public ICollection<AttributeDescriptionBO> AttributeTypes { get; set; } 
        public ICollection<RequirementBO> Requirements { get; set; } 
        public int ProjectId { get; set; }
        public int[] Attributes { get; set; }

  
    }
}