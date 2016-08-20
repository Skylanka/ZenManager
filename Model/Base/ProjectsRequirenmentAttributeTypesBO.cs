using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using Model.BusinessObjects;

namespace Model.Base
{
    public class ProjectsRequirenmentAttributeTypesBO 
    {
        [Key]
        [Column(Order = 0)]
        public int ProjectId { get; set; }   // FK property
        [Key]
        [Column(Order = 1)]
        public int RequirementsDescriptionId { get; set; }  // FK property
        [Key]
        [Column(Order = 2)]
        public int AttributeDescriptionId { get; set; }   // FK property

        public ProjectBO Project { get; set; }
        public RequirementsDescriptionBO RequirementsDescription { get; set; }
        public AttributeDescriptionBO AttributeDescription { get; set; }
    }
}