using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MA_WEB.Controllers.Parameters
{
    public class IdProjectParams : IdParams
    {
        public int ProjectId { get; set; }

        public virtual bool IsNotValid(out string error)
        {
            if (ProjectId <= 0)
            {
                error = "Invalid Id";
                return true;
            }
            error = null;
            return false;
        }
    }
}