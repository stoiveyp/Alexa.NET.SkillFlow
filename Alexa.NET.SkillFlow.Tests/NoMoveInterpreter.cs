using Alexa.NET.SkillFlow.Interpreter;

namespace Alexa.NET.SkillFlow.Tests
{
    public class NoMoveInterpreter : ISkillFlowInterpreter
    {
        public bool CanInterpret(string candidate, SkillFlowInterpretationContext context)
        {
            return candidate.StartsWith('~');
        }

        public int Interpret(string candidate, SkillFlowInterpretationContext context)
        {
            return 0;
        }
    }
}