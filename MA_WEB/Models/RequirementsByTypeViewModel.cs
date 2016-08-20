using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model.BusinessObjects;

namespace MA_WEB.Models
{
    public class RequirementsByTypeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string UniqueTag { get; set; }
        public ICollection<Attributes> Attributes { get; set; }
    }

    public class Attributes
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public int AttrType { get; set; }
        public AttributeType EditType { get; set; }
        public string[] Values { get; set; } 
    }
}