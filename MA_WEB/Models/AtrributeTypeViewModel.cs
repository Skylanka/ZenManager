using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using Model.BusinessObjects;

namespace MA_WEB.Models
{
    public class AtrributeTypeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProjectId { get; set; }
        public string[] Values { get; set; }
        public string Default { get; set; }
        public string Type { get; set; }

        public static implicit operator AttributeDescriptionBO(AtrributeTypeViewModel atrributeTypeViewView)
        {
            return new AttributeDescriptionBO()
            {
                Id = atrributeTypeViewView.Id,
                Name = atrributeTypeViewView.Name,
                Values = atrributeTypeViewView.Values,
                DefaultValue = atrributeTypeViewView.Default,
                Type = atrributeTypeViewView.Type == "List" ? AttributeType.List : AttributeType.Text
            };
        }

        public static implicit operator AtrributeTypeViewModel(AttributeDescriptionBO attributeDescriptionBo)
        {
            return new AtrributeTypeViewModel()
            {
                Id = attributeDescriptionBo.Id,
                Name = attributeDescriptionBo.Name,
                Values = attributeDescriptionBo.Values,
                Default = attributeDescriptionBo.DefaultValue,
                Type = Enum.GetName(typeof(AttributeType), attributeDescriptionBo.Type)
            };
        }
    }
}