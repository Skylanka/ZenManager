using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Base;

namespace Model.BusinessObjects
{
    public class ChatRecordBO:BaseBO
    {
        public ProjectBO Project { get; set; }
        public String Message { get; set; }
        public DateTime Time { get; set; }
        public ApplicationUser User { get; set; }
    }
}
