using System;
using System.Collections.Generic;
using System.Text;

namespace Alexa.NET.SkillFlow.Interpreter
{
    public class CommentInterpreter:ISkillFlowInterpreter
    {
        public bool CanInterpret(string candidate, SkillFlowInterpretationContext context)
        {
            return candidate.StartsWith("//");
        }

        public InterpreterResult Interpret(string candidate, SkillFlowInterpretationContext context)
        {
            context.Comments.Add(candidate.Substring(2));
            return InterpreterResult.Empty;
        }
    }
}
