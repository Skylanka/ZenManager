using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MA_WEB.Controllers.Parameters
{
    public class RequirementProjectParams : IParameters
    {
        [Required]
        public int ProjectId { get; set; }

        [Required]
        public int ReqTypeId { get; set; }


        public virtual bool IsNotValid(out string error)
        {
            if (ProjectId < 0)
            {
                error = "Invalid ProjectId";
                return true;
            }
            if (ReqTypeId < 0)
            {
                error = "Invalid ReqTypeId";
                return true;
            }
            error = null;
            return false;
        }
    }
}