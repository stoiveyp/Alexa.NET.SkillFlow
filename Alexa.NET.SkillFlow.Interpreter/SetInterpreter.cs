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
            return candidate.Length > 5 && candidate.StartsWith("set ") && char.IsNumber(candidate.Last());
        }

        public InterpreterResult Interpret(string candidate, SkillFlowInterpretationContext context)
        {
            var pieces = candidate.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            if (pieces[2] == "to" && int.TryParse(pieces[3], out var value))
            {
                return new InterpreterResult(new Set(pieces[1], value));
            }
            throw new InvalidSkillFlowDefinitionException("Invalid set command", context.LineNumber);
        }
    }
}
