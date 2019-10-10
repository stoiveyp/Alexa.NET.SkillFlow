using System;
using System.Collections.Generic;
using System.Text;

namespace Alexa.NET.SkillFlow.Conditions
{
    public class Not:Condition
    {
        public Condition Condition { get; set; }

        public Not() { }

        public Not(Condition condition)
        {
            Condition = condition;
        }
    }
}
