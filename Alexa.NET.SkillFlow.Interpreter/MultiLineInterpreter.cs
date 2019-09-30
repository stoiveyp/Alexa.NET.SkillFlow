using System;
using System.Collections.Generic;
using System.Text;

namespace Alexa.NET.SkillFlow.Interpreter
{
    public class MultiLineInterpreter:ISkillFlowInterpreter
    {
        public bool CanInterpret(string candidate, SkillFlowInterpretationContext context)
        {
            return context.BeginningOfLine;
        }

        public int Interpret(string candidate, SkillFlowInterpretationContext context)
        {
            context.CurrentComponent.Add(new Text(candidate));
            return candidate.Length;
        }
    }
}
