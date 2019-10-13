using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alexa.NET.SkillFlow.Instructions;

namespace Alexa.NET.SkillFlow.Interpreter
{
    public class HearInterpreter:ISkillFlowInterpreter
    {
        public bool CanInterpret(string candidate, SkillFlowInterpretationContext context)
        {
            return candidate.Length > 7 && candidate.StartsWith("hear ") && candidate.EndsWith("{");
        }

        public InterpreterResult Interpret(string candidate, SkillFlowInterpretationContext context)
        {
            return new InterpreterResult(new Hear(candidate.Substring(5, candidate.Length - 6)
                .Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim())));
        }
    }
}
