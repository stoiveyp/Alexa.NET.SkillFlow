using System;
using System.Collections.Generic;
using System.Text;

namespace Alexa.NET.SkillFlow.Conditions
{
    public class Group:UnaryCondition
    {
        public Group() { }
        public Group(Condition condition)
        {
            Condition = condition;
        }
    }
}
