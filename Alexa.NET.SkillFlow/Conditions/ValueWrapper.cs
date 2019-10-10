namespace Alexa.NET.SkillFlow.Conditions
{
    public class ValueWrapper : Condition
    {
        public Value Value { get; set; }

        public ValueWrapper(){}

        public ValueWrapper(Value value)
        {
            Value = value;
        }
    }
}