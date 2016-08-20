using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Base;

namespace Model.BusinessObjects
{
    public class AttributeBO : BaseBO
    {
        public string Value { get; set; }
        public AttributeDescriptionBO Type { get; set; }
       /* public DateTime CreatedDate { get; set; }*/
    }
}
