using System;
using System.Collections.Generic;
using System.Text;
using Alexa.NET.SkillFlow.Instructions;

namespace Alexa.NET.SkillFlow.Interpreter
{
    public class GoToInterpreter:ISkillFlowInterpreter
    {
        public bool CanInterpret(string candidate, SkillFlowInterpretationContext context)
        {
            return candidate.StartsWith("<->") || candidate.StartsWith("->");
        }

        public InterpreterResult Interpret(string candidate, SkillFlowInterpretationContext context)
        {
            if (candidate.StartsWith("<->"))
            {
                return new InterpreterResult(new GoToAndReturn(candidate.Substring(3).Trim()));
            }
            return new InterpreterResult(new GoTo(candidate.Substring(2).Trim()));
        }
    }
}
