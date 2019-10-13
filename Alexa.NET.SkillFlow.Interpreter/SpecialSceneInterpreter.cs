using System;

namespace Alexa.NET.SkillFlow.Interpreter
{
    public class SpecialSceneInterpreter : ISkillFlowInterpreter
    {
        public bool CanInterpret(string candidate, SkillFlowInterpretationContext context)
        {
            return candidate == "@start"
                   || candidate == "@pause"
                   || candidate == "@resume"
                   || candidate == "@global prepend"
                   || candidate == "@global append";
        }

        public InterpreterResult Interpret(string candidate, SkillFlowInterpretationContext context)
        {
            var sceneName = candidate.Substring(1);
            return new InterpreterResult(new Scene(sceneName));
        }
    }
}
