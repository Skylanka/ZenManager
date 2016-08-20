using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Base;

namespace Model.BusinessObjects
{
    public class IssueBO : BaseDatedBO
    {
        public ApplicationUser AssignedTo { get; set; }
        public IssueStatus Status { get; set; }
        public ICollection<RequirementBO> Requirements { get; set; }
        public ProjectBO Project { get; set; }
        public Priority Priority { get; set; }
        public List<CommentBO> Comments { get; set; }

    }
    public enum IssueStatus
    {
        Open = 0,
        InProgress = 1,
        ClosedNoReproduce = 2,
        FixedResolved = 3
    }

    public enum Priority
    {
        Low = 0,
        Medium = 1,
        High = 2
    }
}
