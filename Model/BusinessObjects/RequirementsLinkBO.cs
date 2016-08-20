using System;
using System.Collections.Generic;
using Model.Base;

namespace Model.BusinessObjects
{
    public class RequirementsLinkBO : BaseBO
    {
        public RequirementBO RequirementFrom { get; set; }
        public RequirementBO RequirementTo { get; set; }
    }
}