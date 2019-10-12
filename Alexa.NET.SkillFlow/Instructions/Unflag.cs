using System;
using System.Collections.Generic;
using System.Text;
using Alexa.NET.SkillFlow.Conditions;

namespace Alexa.NET.SkillFlow.Instructions
{
    public class Unflag : ISceneInstruction
    {
        public string Type => nameof(Unflag);

        public string Variable { get; set; }

        public Unflag() { }
        public Unflag(string variable)
        {
            Variable = variable;
        }

        public void Add(ISkillFlowComponent component)
        {
            throw this.InvalidComponent(component);
        }
    }
}
