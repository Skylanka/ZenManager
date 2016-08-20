using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MA_WEB.Controllers.Parameters
{
    public class IdAttributeParams : IdProjectParams
    {
        public int RequirementTypeId { get; set; }

        public virtual bool IsNotValid(out string error)
        {
            if (RequirementTypeId <= 0)
            {
                error = "Invalid Id";
                return true;
            }
            error = null;
            return false;
        }
    }
}