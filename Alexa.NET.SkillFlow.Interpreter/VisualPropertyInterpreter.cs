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
            return candidate.IndexOf(':') > -1 && candidate.IndexOfAny(quoters) > -1;
        }

        public InterpreterResult Interpret(string candidate, SkillFlowInterpretationContext context)
        {
            var keyvalue = candidate.Split(new[] {':'}, 2).Select(s => s.Trim()).ToArray();

            if (keyvalue.Length < 2)
            {
                throw new InvalidSkillFlowDefinitionException($"Unable to interpret visual property {candidate}", context.LineNumber);
            }

            if (!_validProperties.Contains(keyvalue[0]) || !quoters.Contains(keyvalue[1][0]) || !quoters.Contains(keyvalue[1].Last()))
            {
                throw new InvalidSkillFlowDefinitionException($"Unable to interpret visual property {keyvalue[0]}",context.LineNumber);
            }

            var property = new VisualProperty(keyvalue[0], keyvalue[1].Substring(1, keyvalue[1].Length - 2));

            return new InterpreterResult(property);
        }
    }
}
