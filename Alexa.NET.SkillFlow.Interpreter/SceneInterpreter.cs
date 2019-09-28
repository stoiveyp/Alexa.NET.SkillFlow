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

        public int Interpret(string candidate, SkillFlowInterpretationContext context)
        {
            if (candidate.Length <= 7)
            {
                throw new InvalidSkillFlowException($"No scene name", context.LineNumber);
            }

            var sceneName = candidate.Substring(7);

            if (!sceneName.All(c => char.IsLetterOrDigit(c) || c == ' '))
            {
                throw new InvalidSkillFlowException($"Invalid scene name '{sceneName}'", context.LineNumber);
            }

            context.Story.Scenes.Add(sceneName, new Scene { Name = sceneName });
            return 7 + sceneName.Length;
        }
    }
}
