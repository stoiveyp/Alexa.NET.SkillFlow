using System;
using System.Collections.Generic;
using System.Text;

namespace Alexa.NET.SkillFlow.Conditions
{
    public class LiteralValue:Value
    {
        public object Value { get; set; }
        public LiteralValue() { }

        public LiteralValue(object value)
        {
            Value = value;
        }
    }
}
