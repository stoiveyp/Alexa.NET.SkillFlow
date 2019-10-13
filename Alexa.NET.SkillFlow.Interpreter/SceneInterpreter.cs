using System;
using System.Linq;

namespace Alexa.NET.SkillFlow.Interpreter
{
    public class SceneInterpreter : ISkillFlowInterpreter
    {
        public bool CanInterpret(string candidate, SkillFlowInterpretationContext context)
        {
            return candidate.Length > 3 && candidate[0] == '@' &&
                   candidate.Skip(1).All(c => char.IsLetterOrDigit(c) || c == ' ');
        }

        public InterpreterResult Interpret(string candidate, SkillFlowInterpretationContext context)
        {
            return new InterpreterResult(new Scene(candidate.Substring(1)));
        }
    }
}
