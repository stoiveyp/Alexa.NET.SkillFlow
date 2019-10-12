namespace Alexa.NET.SkillFlow.Terminators
{
    public class Pause : ISceneTerminator
    {
        public string Type => nameof(Pause);
        public void Add(ISkillFlowComponent component)
        {
            throw this.InvalidComponent(component);
        }
    }
}