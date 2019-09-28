namespace Alexa.NET.SkillFlow.Interpreter
{
    public interface ISkillFlowInterpreter
    {
        bool CanInterpret(string candidate, SkillFlowInterpretationContext context);
        int Interpret(string candidate, SkillFlowInterpretationContext context);
    }
}