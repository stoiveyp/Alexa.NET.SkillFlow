using System;
using System.Collections.Generic;
using System.Text;

namespace Alexa.NET.SkillFlow.Interpreter
{
    public class InvalidSkillFlowDefinitionException:Exception
    {
        public InvalidSkillFlowDefinitionException(string message, int lineNumber)
        :base($"{lineNumber}: {message}")
        {
            LineNumber = lineNumber;
        }

        public int LineNumber { get; set; }
    }
}
