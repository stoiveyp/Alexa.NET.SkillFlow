using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alexa.NET.SkillFlow.Interpreter
{
    public class VisualPropertyInterpreter : ISkillFlowInterpreter
    {
        private readonly char[] quoters = { '"', '\'' };
        private readonly string[] _validProperties = { "background", "template", "title", "subtitle" };
        public bool CanInterpret(string candidate, SkillFlowInterpretationContext context)
        {
            return context.CurrentComponent is Visual && candidate.IndexOf(':') > -1 && candidate.IndexOfAny(quoters) > -1;
        }

        public InterpreterResult Interpret(string candidate, SkillFlowInterpretationContext context)
        {
            var keyvalue = candidate.Split(new[] {':'}, 2);

            if (keyvalue.Length < 2)
            {
                return InterpreterResult.Empty;
            }

            if (!_validProperties.Contains(keyvalue[0]))
            {
                throw new InvalidSkillFlowDefinitionException($"Unable to recognise visual property {keyvalue[0]}",context.LineNumber);
            }

            return InterpreterResult.Empty;
        }
    }
}
