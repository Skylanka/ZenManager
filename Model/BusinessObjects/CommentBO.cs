using System;
using System.Threading;
using Model.Base;

namespace Model.BusinessObjects
{
    public class CommentBO : BaseBO
    {
        public String Text { get; set; }
        public RequirementBO Requirement { get; set; }
        public IssueBO Issue { get; set; }
        public DateTime Created { get; set; }
        public ApplicationUser CreatedBy { get; set; }
    }

    public class CopyOfCommentBO : BaseBO
    {
        public String Text { get; set; }
        public RequirementBO Requirement { get; set; }
        public IssueBO Issue { get; set; }
        public DateTime Created { get; set; }
    }
}