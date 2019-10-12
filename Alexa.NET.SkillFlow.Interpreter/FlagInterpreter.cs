using System;
using System.Collections.Generic;
using System.Text;
using Alexa.NET.SkillFlow.Instructions;

namespace Alexa.NET.SkillFlow.Interpreter
{
    public class FlagInterpreter : ISkillFlowInterpreter
    {
        public bool CanInterpret(string candidate, SkillFlowInterpretationContext context)
        {
            return candidate.Length > 5 && candidate.IndexOf(' ') == candidate.LastIndexOf(' ')
                                        && (candidate.StartsWith("flag ") || candidate.StartsWith("unflag "));
        }

        public InterpreterResult Interpret(string candidate, SkillFlowInterpretationContext context)
        {
            var splits = candidate.Split(new[] { ' ' }, 2);
            if (splits[0] == "flag")
            {
                return new InterpreterResult(new Flag(splits[1]));
            }

            if (splits[0] == "unflag")
            {
                return new InterpreterResult(new Unflag(splits[1]));
            }

            throw new InvalidSkillFlowDefinitionException("Invalid flag statement", context.LineNumber);
        }
    }
}
