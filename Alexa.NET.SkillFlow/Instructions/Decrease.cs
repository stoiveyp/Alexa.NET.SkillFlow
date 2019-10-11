namespace Alexa.NET.SkillFlow.Instructions
{
    public class Decrease : ISceneInstruction
    {
        public Decrease(string variable, int amount)
        {
            Variable = variable;
            Amount = amount;
        }

        public int Amount { get; set; }

        public string Variable { get; set; }

        public string Type => nameof(Decrease);
        public void Add(ISkillFlowComponent component)
        {
            throw this.InvalidComponent(component);
        }
    }
}