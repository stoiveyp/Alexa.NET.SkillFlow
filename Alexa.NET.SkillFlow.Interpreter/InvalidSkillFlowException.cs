using System;
using System.Collections.Generic;
using System.Text;

namespace Alexa.NET.SkillFlow.Interpreter
{
    public class InvalidSkillFlowException:Exception
    {
        public InvalidSkillFlowException(string message, int lineNumber):base(message)
        {
            LineNumber = lineNumber;
        }

        public int LineNumber { get; set; }
    }
}
