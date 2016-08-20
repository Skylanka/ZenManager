using System.Collections.Generic;
using Model.Base;

namespace Model.BusinessObjects
{
    public class RequirementsDescriptionBO : BaseNamedBO
    {
        public string Tag { get; set; }
        public ICollection<ProjectsRequirenmentAttributeTypesBO> ProjectsRequirenmentAttributeTypes { get; set; }
        public ProjectBO Project { get; set; }
    }
}