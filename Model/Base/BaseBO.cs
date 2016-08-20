using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Base
{
    public class BaseBO
    {
        [Key]
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
    }
}