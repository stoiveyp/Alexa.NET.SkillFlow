using System;
using System.Collections.Generic;
using System.Text;

namespace Alexa.NET.SkillFlow.Conditions
{
    public class Variable:Value
    {
        public string Name { get; set; }

        public Variable() { }

        public Variable(string name)
        {
            Name = name;
        }
    }
}
