using System;
using System.Collections.Generic;
using System.Text;
using Alexa.NET.SkillFlow.Instructions;

namespace Alexa.NET.SkillFlow.Interpreter
{
    public class IfInterpreter:ISkillFlowInterpreter
    {
        public bool CanInterpret(string candidate, SkillFlowInterpretationContext context)
        {
            return candidate.Length > 5 && candidate.StartsWith("if ") && candidate.EndsWith("{");
        }

        public InterpreterResult Interpret(string candidate, SkillFlowInterpretationContext context)
        {
            var conditionText = candidate.Substring(3, candidate.Length - 4);
            var condition = ConditionParser.Parse(conditionText);
            return new InterpreterResult(new If(condition));
            
        }
    }
}
