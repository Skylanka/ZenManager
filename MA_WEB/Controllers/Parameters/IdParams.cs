using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MA_WEB.Controllers.Parameters
{
    public class IdParams : IParameters
    {
        public int Id { get; set; }

        public virtual bool IsNotValid(out string error)
        {
            if (Id <= 0)
            {
                error = "Invalid Id";
                return true;
            }
            error = null;
            return false;
        }
    }
}