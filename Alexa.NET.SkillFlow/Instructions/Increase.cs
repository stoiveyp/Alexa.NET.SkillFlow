using System;
using System.Collections.Generic;
using System.Text;

namespace Alexa.NET.SkillFlow.Instructions
{
    public class Increase:SceneInstruction
    {
        public Increase(string variable, int amount)
        {
            Variable = variable;
            Amount = amount;
        }

        public int Amount { get; set; }

        public string Variable { get; set; }

        public override string Type => nameof(Increase);
    }
}
