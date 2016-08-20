using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MA_WEB.Models
{
    public class InvitationViewModel
    {
        public int Id { get; set; }
        public int Status { get; set; }
        public int ProjectId { get; set; }
        public string Email { get; set; }
        public string ProjectName { get; set; }
    }
}