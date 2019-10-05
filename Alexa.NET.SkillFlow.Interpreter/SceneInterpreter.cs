using System;
using System.Linq;

namespace Alexa.NET.SkillFlow.Interpreter
{
    public class SceneInterpreter : ISkillFlowInterpreter
    {
        public bool CanInterpret(string candidate, SkillFlowInterpretationContext context)
        {
            return candidate.StartsWith("@scene");
        }

        public InterpreterResult Interpret(string candidate, SkillFlowInterpretationContext context)
        {
            if (candidate.Length <= 7)
            {
                throw new InvalidSkillFlowDefinitionException($"No scene name", context.LineNumber);
            }

            var sceneName = candidate.Substring(7);

            if (!sceneName.All(c => char.IsLetterOrDigit(c) || c == ' '))
            {
                throw new InvalidSkillFlowDefinitionException($"Invalid scene name '{sceneName}'", context.LineNumber);
            }

            return new InterpreterResult(7 + sceneName.Length, new Scene(sceneName));
        }
    }
}
