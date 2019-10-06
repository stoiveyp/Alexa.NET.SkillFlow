namespace Alexa.NET.SkillFlow.Conditions
{
    public class Wrapper : Condition
    {
        public Condition Condition { get; set; }

        public Wrapper(){}

        public Wrapper(Condition condition)
        {
            Condition = condition;
        }
    }
}