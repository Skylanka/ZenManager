using System;

namespace Model.Base
{
    public class BaseDatedBO : BaseNamedBO
    {
        public DateTime Created { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public DateTime Updated { get; set; }
        public ApplicationUser UpdatedBy { get; set; }
    }
}