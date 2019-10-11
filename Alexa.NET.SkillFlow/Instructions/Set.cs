using System;
using System.Collections.Generic;
using System.Text;

namespace Alexa.NET.SkillFlow.Instructions
{
    public class Set:ISceneInstruction
    {
        public Set() { }

        public Set(string variable, int amount)
        {
            Variable = variable;
            Amount = amount;
        }
        public string Variable { get; set; }
        public int Amount { get; set; }
        public string Type => nameof(Set);
        public void Add(ISkillFlowComponent component)
        {
            throw this.InvalidComponent(component);
        }
    }
}
