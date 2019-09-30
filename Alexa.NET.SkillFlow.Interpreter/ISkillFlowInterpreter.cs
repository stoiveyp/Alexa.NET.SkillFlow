namespace Alexa.NET.SkillFlow.Interpreter
{
    public interface ISkillFlowInterpreter
    {
        bool CanInterpret(string candidate, SkillFlowInterpretationContext context);
        (int Used,ISkillFlowComponent Component) Interpret(string candidate, SkillFlowInterpretationContext context);
    }
}