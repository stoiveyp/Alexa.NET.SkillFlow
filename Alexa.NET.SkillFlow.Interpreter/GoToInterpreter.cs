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
            return candidate == "<->" || candidate.StartsWith("->");
        }

        public InterpreterResult Interpret(string candidate, SkillFlowInterpretationContext context)
        {
            if (candidate == "<->")
            {
                return new InterpreterResult(new GoToAndReturn());
            }
            var sceneName = candidate.Substring(2).Trim();
            return new InterpreterResult(new GoTo(sceneName));
        }
    }
}
