using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alexa.NET.SkillFlow.Instructions;

namespace Alexa.NET.SkillFlow.Interpreter
{
    public class SlotAssignmentInterpreter:ISkillFlowInterpreter
    {
        public bool CanInterpret(string candidate, SkillFlowInterpretationContext context)
        {
            return candidate.StartsWith("slot ") && candidate.Contains(" to ") && candidate.Last() == '\'';
        }

        public InterpreterResult Interpret(string candidate, SkillFlowInterpretationContext context)
        {
            var pieces = candidate.Split(new[] { ' ' }, 4,StringSplitOptions.RemoveEmptyEntries);
            if (pieces[2] == "to" && pieces[3][0] == '\'')
            {
                return new InterpreterResult(new SlotAssignment(pieces[1], pieces[3].Trim('\'')));
            }

            throw new InvalidSkillFlowDefinitionException("Invalid slot assignmnet command", context.LineNumber);
        }
    }
}
