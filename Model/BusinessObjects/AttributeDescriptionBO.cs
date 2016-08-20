using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Text;
using Model.Base;
using Newtonsoft.Json;

namespace Model.BusinessObjects
{
    public class AttributeDescriptionBO : BaseNamedBO
    {
        public AttributeType Type { get; set; }
        public string InternalValue { get; set; }
        public ICollection<ProjectsRequirenmentAttributeTypesBO> ProjectsRequirenmentAttributeTypes { get; set; }
        public string DefaultValue { get; set; }
        public bool IsDefault { get; set; }
        public ProjectBO Project { get; set; }


        private string[] _value;

        [NotMapped]
        public string[] Values
        {
            get
            {
                if (_value == null)
                {
                    if (InternalValue == null)
                    {
                        _value = new string[] {};
                    }
                    else
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        using (TextReader txtReader = new StringReader(InternalValue))
                        {
                            JsonReader reader = new JsonTextReader(txtReader);
                            _value = serializer.Deserialize<string[]>(reader);
                        }
                    }

                }
                return _value;
            }
            set
            {
                _value = value;

                JsonSerializer serializer = new JsonSerializer();
                StringBuilder sb = new StringBuilder();
                using (TextWriter writer = new StringWriter(sb))
                {
                    serializer.Serialize(writer, value);
                }

                InternalValue = sb.ToString();
            }
        }
    }

    public enum AttributeType
    {
        Text = 0,
        List = 1
    }
}