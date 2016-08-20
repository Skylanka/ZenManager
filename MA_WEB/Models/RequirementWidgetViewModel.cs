using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using Model.BusinessObjects;

namespace MA_WEB.Models
{
    public class RequirementWidgetViewModel
    {
        public ICollection<RequirementWidgetViewModelItem> Types { get; set; }
    }

    public class RequirementWidgetViewModelItem
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Type { get; set; }
        public int Count { get; set; }
        public int LastUpdated { get; set; }
    }
}