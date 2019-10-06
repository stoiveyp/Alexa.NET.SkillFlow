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
            return context.CurrentComponent is SceneInstructions && candidate.StartsWith("->");
        }

        public InterpreterResult Interpret(string candidate, SkillFlowInterpretationContext context)
        {
            var sceneName = candidate.Substring(2).Trim();
            return new InterpreterResult(new GoTo(sceneName));
        }
    }
}
