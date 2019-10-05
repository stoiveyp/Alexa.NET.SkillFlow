using System;
using System.Collections.Generic;
using System.Text;

namespace Alexa.NET.SkillFlow.Interpreter
{
    public class MultiLineInterpreter:ISkillFlowInterpreter
    {
        public bool CanInterpret(string candidate, SkillFlowInterpretationContext context)
        {
            return context.BeginningOfLine && context.CurrentComponent is Text;
        }

        public InterpreterResult Interpret(string candidate, SkillFlowInterpretationContext context)
        {
            return candidate == "||" 
                ? new InterpreterResult(candidate.Length,new Variation()) 
                : new InterpreterResult(candidate.Length,new TextLine(candidate));
        }
    }
}
