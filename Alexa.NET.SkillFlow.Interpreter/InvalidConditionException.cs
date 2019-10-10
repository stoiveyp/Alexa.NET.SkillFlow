using System;
using System.Collections.Generic;
using System.Text;

namespace Alexa.NET.SkillFlow.Interpreter
{
    public class InvalidConditionException:Exception
    {
        public InvalidConditionException(string condition) : base($"Unable to parse condition '{condition}'") { }
    }
}
