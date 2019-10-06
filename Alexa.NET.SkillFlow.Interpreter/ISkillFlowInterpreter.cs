namespace Alexa.NET.SkillFlow.Interpreter
{
    public interface ISkillFlowInterpreter
    {
        bool CanInterpret(string candidate, SkillFlowInterpretationContext context);
        InterpreterResult Interpret(string candidate, SkillFlowInterpretationContext context);
    }
}