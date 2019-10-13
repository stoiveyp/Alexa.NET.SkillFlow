using System;
using System.Collections.Generic;
using System.Text;

namespace Alexa.NET.SkillFlow.Conditions
{
    public abstract class UnaryCondition:Condition
    {
        public Value Condition { get; set; }
    }
}
