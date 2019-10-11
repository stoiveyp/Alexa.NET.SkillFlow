using System;
using System.Collections.Generic;
using System.Text;

namespace Alexa.NET.SkillFlow.Instructions
{
    public class Increase:ISceneInstruction
    {
        public Increase(string variable, int amount)
        {
            Variable = variable;
            Amount = amount;
        }

        public int Amount { get; set; }

        public string Variable { get; set; }

        public string Type => nameof(Increase);
        public void Add(ISkillFlowComponent component)
        {
            throw this.InvalidComponent(component);
        }
    }
}
