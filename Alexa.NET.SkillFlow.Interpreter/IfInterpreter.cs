using System;
using System.Collections.Generic;
using System.Text;

namespace Alexa.NET.SkillFlow.Interpreter
{
    public class IfInterpreter:ISkillFlowInterpreter
    {
        public bool CanInterpret(string candidate, SkillFlowInterpretationContext context)
        {
            return candidate.Length > 5 && candidate.StartsWith("if ") && candidate.EndsWith("{");
        }

        public InterpreterResult Interpret(string candidate, SkillFlowInterpretationContext context)
        {
            return InterpreterResult.Empty;
        }
    }
}
