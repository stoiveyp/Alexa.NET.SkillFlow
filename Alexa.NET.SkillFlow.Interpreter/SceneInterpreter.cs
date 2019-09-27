using System;

namespace Alexa.NET.SkillFlow.Interpreter
{
    public class SceneInterpreter:ISkillFlowInterpreter
    {
        public bool CanInterpret(string candidate, SkillFlowInterpretationContext context)
        {
            return candidate[0] == '@';
        }

        public int Interpret(string candidate, SkillFlowInterpretationContext context)
        {
            throw new NotImplementedException();
        }
    }
}
