using System;
using System.Collections.Generic;
using System.Text;

namespace Alexa.NET.SkillFlow.Interpreter
{
    public class SceneInstructionInterpreter:ISkillFlowInterpreter
    {
        public bool CanInterpret(string candidate, SkillFlowInterpretationContext context)
        {
            return false;
        }

        public InterpreterResult Interpret(string candidate, SkillFlowInterpretationContext context)
        {
            return InterpreterResult.Empty;
        }
    }
}
