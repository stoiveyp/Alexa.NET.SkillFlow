namespace Alexa.NET.SkillFlow.Terminators
{
    public class End : ISceneTerminator
    {
        public string Type => nameof(End);
        public void Add(ISkillFlowComponent component)
        {
            throw this.InvalidComponent(component);
        }
    }
}