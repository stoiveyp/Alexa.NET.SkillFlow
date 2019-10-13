using System;
using System.Collections.Generic;
using System.Text;

namespace Alexa.NET.SkillFlow.Instructions
{
    public class Set:SceneInstruction
    {
        public Set() { }

        public Set(string variable, object value)
        {
            Variable = variable;
            Value = value;
        }
        public string Variable { get; set; }
        public object Value { get; set; }
        public override string Type => nameof(Set);
    }
}
