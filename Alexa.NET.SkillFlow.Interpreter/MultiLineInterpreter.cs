using System;
using System.Collections.Generic;
using System.Text;

namespace Alexa.NET.SkillFlow.Interpreter
{
    public class MultiLineInterpreter:ISkillFlowInterpreter
    {
        public bool CanInterpret(string candidate, SkillFlowInterpretationContext context)
        {
            return context.BeginningOfLine && context.CurrentComponent is Text;
        }

        public (int Used, ISkillFlowComponent Component) Interpret(string candidate, SkillFlowInterpretationContext context)
        {
            if (candidate == "||")
            {
                return (candidate.Length,new Variation());
            }
            return (candidate.Length,new TextLine(candidate));
        }
    }
}
