using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MA_WEB.Controllers.Parameters
{
    public class PagingParams : IParameters
    {
        [Required]
        public int Page { get; set; }

        [Required]
        public int PageSize { get; set; }


        public virtual bool IsNotValid(out string error)
        {
            if (Page < 0)
            {
                error = "Invalid page";
                return true;
            }
            if (PageSize <= 0 || PageSize > 200)
            {
                error = "Invalid page size";
                return true;
            }
            error = null;
            return false;
        }
    }
}