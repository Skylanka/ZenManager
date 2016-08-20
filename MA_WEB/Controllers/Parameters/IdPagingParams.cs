using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MA_WEB.Controllers.Parameters
{
    public class IdPagingParams : PagingParams
    {
        [Required]
        public int Id { get; set; }

        public override bool IsNotValid(out string error)
        {
            if (Id <= 0)
            {
                error = "Invalid Id";
                return true;
            }

            return base.IsNotValid(out error);
        }
    }
}