namespace Alexa.NET.SkillFlow.Instructions
{
    public class Decrease : SceneInstruction
    {
        public Decrease(string variable, int amount)
        {
            Variable = variable;
            Amount = amount;
        }

        public int Amount { get; set; }

        public string Variable { get; set; }

        public override string Type => nameof(Decrease);
    }
}