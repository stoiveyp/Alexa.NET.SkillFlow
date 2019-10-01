using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alexa.NET.SkillFlow.Interpreter
{
    public class ScenePropertyInterpreter : ISkillFlowInterpreter
    {
        readonly string[] TextWords = { "recap", "say", "reprompt" };
        public bool CanInterpret(string candidate, SkillFlowInterpretationContext context)
        {
            var property = candidate.Substring(1);
            return candidate[0] == '*' &&
                   (TextWords.Contains(property)
                    || candidate == "show"
                    || candidate == "then");


        }

        public (int Used, ISkillFlowComponent Component) Interpret(string candidate, SkillFlowInterpretationContext context)
        {
            var property = candidate.Substring(1);
            switch (property)
            {
                case "show":
                    return (candidate.Length,new Visual());
                default:
                    return (candidate.Length, new Text(candidate.Substring(1)));
            }
        }
    }
}
