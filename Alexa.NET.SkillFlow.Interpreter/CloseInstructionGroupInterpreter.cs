using System;
using System.Collections.Generic;
using System.Text;

namespace Alexa.NET.SkillFlow.Interpreter
{
    public class CloseInstructionGroupInterpreter : ISkillFlowInterpreter
    {
        public bool CanInterpret(string candidate, SkillFlowInterpretationContext context)
        {
            return candidate == "}";
        }

        public InterpreterResult Interpret(string candidate, SkillFlowInterpretationContext context)
        {
            if (context.CurrentComponent is SceneInstructionContainer container && container.Group)
            {
                context.Components.Pop();
                return InterpreterResult.Empty;
            }

            throw new InvalidSkillFlowDefinitionException("Invalid indentation = unable to determine group", context.LineNumber);
        }
    }
}
