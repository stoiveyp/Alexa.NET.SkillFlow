using System;
using System.Collections.Generic;
using System.Text;

namespace Alexa.NET.SkillFlow
{
    public class VisualProperty : ISkillFlowComponent
    {
        public VisualProperty() { }

        public VisualProperty(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; set; }

        public string Value { get; set; }
        public string Type => nameof(VisualProperty);
        public void Add(ISkillFlowComponent component)
        {
            throw new NotImplementedException();
        }
    }
}
