using System;
using System.Collections.Generic;
using System.Text;

namespace Alexa.NET.SkillFlow.Instructions
{
    public class Clear : ISceneInstruction
    {
        public string Variable { get; set; }

        public Clear() { }

        public Clear(string variable)
        {
            Variable = variable;
        }

        public string Type => nameof(Clear);
        public void Add(ISkillFlowComponent component)
        {
            throw this.InvalidComponent(component);
        }
    }
}
