using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alexa.NET.SkillFlow.Instructions;

namespace Alexa.NET.SkillFlow.Interpreter
{
    public class SetInterpreter: ISkillFlowInterpreter
    {
        public bool CanInterpret(string candidate, SkillFlowInterpretationContext context)
        {
            return candidate.Length > 5 && candidate.StartsWith("set ") && (candidate.Contains(" to ") || candidate.Contains(" as "));
        }

        public InterpreterResult Interpret(string candidate, SkillFlowInterpretationContext context)
        {
            var pieces = candidate.Split(new[] {' '}, 4,StringSplitOptions.RemoveEmptyEntries);
            if (pieces[2] == "to" || pieces[2] == "as")
            {
                return new InterpreterResult(new Set(pieces[1], pieces[3]));
            }

            throw new InvalidSkillFlowDefinitionException("Invalid set command", context.LineNumber);
        }
    }
}
