namespace Alexa.NET.SkillFlow.Conditions
{
    public abstract class BinaryCondition : Condition
    {
        public Value Left { get; set; }
        public Value Right { get; set; }
    }
}