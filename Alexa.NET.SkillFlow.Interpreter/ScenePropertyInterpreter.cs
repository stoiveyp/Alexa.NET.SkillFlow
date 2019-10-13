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
                    || property == "show"
                    || property == "then");
        }

        public InterpreterResult Interpret(string candidate, SkillFlowInterpretationContext context)
        {
            if (context.CurrentComponent is Text)
            {
                context.Components.Pop();
            }
            var property = candidate.Substring(1);
            switch (property)
            {
                case "show":
                    return new InterpreterResult(new Visual());
                case "then":
                    return new InterpreterResult(new SceneInstructions());
                default:
                    return new InterpreterResult(new Text(candidate.Substring(1)));
            }
        }
    }
}
