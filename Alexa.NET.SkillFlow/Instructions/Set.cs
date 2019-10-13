using System;
using System.Collections.Generic;
using System.Text;

namespace Alexa.NET.SkillFlow.Instructions
{
    public class Set:SceneInstruction
    {
        public Set() { }

        public Set(string variable, int amount)
        {
            Variable = variable;
            Amount = amount;
        }
        public string Variable { get; set; }
        public int Amount { get; set; }
        public override string Type => nameof(Set);
    }
}
