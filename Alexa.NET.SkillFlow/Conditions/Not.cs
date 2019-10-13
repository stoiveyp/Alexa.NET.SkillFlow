using System;
using System.Collections.Generic;
using System.Text;

namespace Alexa.NET.SkillFlow.Conditions
{
    public class Not:UnaryCondition
    {
        public Not() { }

        public Not(Condition condition)
        {
            Condition = condition;
        }
    }
}
