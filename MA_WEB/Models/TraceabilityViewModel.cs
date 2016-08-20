using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model.BusinessObjects;

namespace MA_WEB.Models
{
    public class TraceabilityViewModel
    {
        public List<TraceabilityObject> Rows { get; set; }
        public List<TraceabilityObject> Columns { get; set; }
    }

    public class TraceabilityObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int[] Links { get; set; }

        public static implicit operator TraceabilityObject(RequirementBO req)
        {
            return new TraceabilityObject()
            {
                Id = req.Id,
                Name = req.UniqueTag + ": " + req.Name,
                Links = new int[] {}
            };
        }
    }
}