using System;
using System.Collections.Generic;
using System.Text;

namespace Alexa.NET.SkillFlow.Instructions
{
    public class Flag : SceneInstruction
    {
        public override string Type => nameof(Flag);

        public string Variable { get; set; }

        public Flag() { }
        public Flag(string variable)
        {
            Variable = variable;
        }
    }
}
