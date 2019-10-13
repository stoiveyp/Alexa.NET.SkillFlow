using System;
using System.Collections.Generic;
using System.Text;
using Alexa.NET.SkillFlow.Instructions;

namespace Alexa.NET.SkillFlow.Interpreter
{
    public class ClearInterpreter : ISkillFlowInterpreter
    {
        public bool CanInterpret(string candidate, SkillFlowInterpretationContext context)
        {
            return candidate.Length > 7 
                   && candidate.IndexOf(' ') == candidate.LastIndexOf(' ') 
                   && candidate.StartsWith("clear ");
        }

        public InterpreterResult Interpret(string candidate, SkillFlowInterpretationContext context)
        {
            var splits = candidate.Split(new[] { ' ' }, 2);
            return splits[1] == "*" ? new InterpreterResult(new ClearAll()) : new InterpreterResult(new Clear(splits[1]));
        }
    }
}
