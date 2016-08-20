using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MA_WEB.Controllers.Parameters
{
    public class TraceabilityParams : IParameters
    {
        public int IdFrom { get; set; }
        public int IdTo { get; set; }

        public virtual bool IsNotValid(out string error)
        {
            if (IdFrom <= 0)
            {
                error = "Invalid IdFrom";
                return true;
            }
            if (IdTo <= 0)
            {
                error = "Invalid IdTo";
                return true;
            }
            error = null;
            return false;
        }
    }
}