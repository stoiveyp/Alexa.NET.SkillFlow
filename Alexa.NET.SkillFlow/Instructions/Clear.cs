using System;
using System.Collections.Generic;
using System.Text;

namespace Alexa.NET.SkillFlow.Instructions
{
    public class Clear : SceneInstruction
    {
        public string Variable { get; set; }

        public Clear() { }

        public Clear(string variable)
        {
            Variable = variable;
        }

        public override string Type => nameof(Clear);
    }
}
