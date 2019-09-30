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

        public (int Used, ISkillFlowComponent Component) Interpret(string candidate, SkillFlowInterpretationContext context)
        {
            return (candidate.Length,new Text(candidate));
        }
    }
}
