namespace Alexa.NET.SkillFlow.Terminators
{
    public class Repeat : ISceneTerminator
    {
        public string Type => nameof(Repeat);
        public void Add(ISkillFlowComponent component)
        {
            throw this.InvalidComponent(component);
        }
    }
}