using System;
using System.Collections.Generic;
using System.Text;

namespace Alexa.NET.SkillFlow.Interpreter
{
    public class VisualPropertyInterpreter:ISkillFlowInterpreter
    {
        private readonly char[] quoters = {'"', '\''};
        public bool CanInterpret(string candidate, SkillFlowInterpretationContext context)
        {
            return context.CurrentComponent is Visual && candidate.IndexOf(':') > -1 && candidate.IndexOfAny(quoters) > -1;
        }

        public (int Used, ISkillFlowComponent Component) Interpret(string candidate, SkillFlowInterpretationContext context)
        {
            throw new InvalidSkillFlowException("Cannot add components to a visual property");
        }
    }
}
