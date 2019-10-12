using System;
using System.Collections.Generic;
using System.Text;

namespace Alexa.NET.SkillFlow.Instructions
{
    public class Flag : ISceneInstruction
    {
        public string Type => nameof(Flag);

        public string Variable { get; set; }

        public Flag() { }
        public Flag(string variable)
        {
            Variable = variable;
        }

        public void Add(ISkillFlowComponent component)
        {
            throw this.InvalidComponent(component);
        }
    }
}
