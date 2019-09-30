using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alexa.NET.SkillFlow.Interpreter
{
    public class ScenePropertyInterpreter : ISkillFlowInterpreter
    {
        string[] CaptureWord = new[] { "recap", "say", "reprompt" };
        public bool CanInterpret(string candidate, SkillFlowInterpretationContext context)
        {
            return candidate[0] == '*' && CaptureWord.Contains(candidate.Substring(1));
        }

        public (int Used, ISkillFlowComponent Component) Interpret(string candidate, SkillFlowInterpretationContext context)
        {
            return (candidate.Length,new Text(candidate.Substring(1)));
        }
    }
}
