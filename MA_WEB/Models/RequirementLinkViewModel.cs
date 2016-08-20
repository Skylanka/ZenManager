using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model.BusinessObjects;

namespace MA_WEB.Models
{
    public class RequirementLinkViewModel
    {
        public int LinkId { get; set; }
        public int ReqId { get; set; }
        public string Tag { get; set; }
        public string Name { get; set; }
        public int SpecId { get; set; }
        public string Specification { get; set; }
        public String Type { get; set; }
    
    /*public static implicit operator RequirementsLinkBO(RequirementLinkViewModel link)
    {
        return new RequirementsLinkBO()
        {
            
        };
    }

    public static implicit operator RequirementLinkViewModel(RequirementsLinkBO link)
    {

        return new RequirementLinkViewModel()
        {
            LinkId = link.Id,
            ReqId = link.R
        };
    }*/
    }
    public enum RelationType
    {
        Child = 0,
        Parent = 1
    }
}