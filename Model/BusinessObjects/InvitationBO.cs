using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Base;

namespace Model.BusinessObjects
{
    public class InvitationBO : BaseBO
    {
        public Guid Code { get; set; }
        public String Email { get; set; }
        public Boolean Status { get; set; }
    }
}
