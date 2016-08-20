using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;

namespace MA_WEB.Controllers.Parameters
{
    public class EmailParams : IParameters
    {
        [Required]
        public int ProjectId { get; set; }
        [Required]
        public string Email { get; set; }

       
        public virtual bool IsNotValid(out string error)
        {

            if (ProjectId <= 0)
            {
                error = "Invalid ProjectId";
                return true;
            }
            if (Email == null)
            {
                error = "Invalid email";
                return true;
            }
            
            error = null;
            return false;
        }
    }
}