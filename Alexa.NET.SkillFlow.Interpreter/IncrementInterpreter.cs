using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alexa.NET.SkillFlow.Instructions;

namespace Alexa.NET.SkillFlow.Interpreter
{
    public class IncrementInterpreter:ISkillFlowInterpreter
    {
        public bool CanInterpret(string candidate, SkillFlowInterpretationContext context)
        {
            return candidate.Length > 13 && (candidate.StartsWith("increase ") || candidate.StartsWith("decrease ")) && char.IsNumber(candidate.Last());
        }

        public InterpreterResult Interpret(string candidate, SkillFlowInterpretationContext context)
        {
            var pieces = candidate.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (pieces[2] == "by" && int.TryParse(pieces[3], out var value))
            {
                if (pieces[0] == "increase")
                {
                    return new InterpreterResult(new Increase(pieces[1], value));
                }
                
                return new InterpreterResult(new Decrease(pieces[1], value));
            }
            throw new InvalidSkillFlowDefinitionException("Invalid increment command", context.LineNumber);
        }
    }
}
