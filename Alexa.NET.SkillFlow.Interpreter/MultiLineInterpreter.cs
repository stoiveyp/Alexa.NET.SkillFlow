using System;
using System.Collections.Generic;
using System.Text;

namespace Alexa.NET.SkillFlow.Interpreter
{
    public class MultiLineInterpreter:ISkillFlowInterpreter
    {
        public bool CanInterpret(string candidate, SkillFlowInterpretationContext context)
        {
            return true;
        }

        public InterpreterResult Interpret(string candidate, SkillFlowInterpretationContext context)
        {
            return candidate == "||" 
                ? new InterpreterResult(new Variation()) 
                : new InterpreterResult(new TextLine(candidate));
        }
    }
}
