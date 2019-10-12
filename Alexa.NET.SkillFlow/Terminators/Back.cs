namespace Alexa.NET.SkillFlow.Terminators
{
    public class Back : ISceneTerminator
    {
        public string Type => nameof(Back);
        public void Add(ISkillFlowComponent component)
        {
            throw this.InvalidComponent(component);
        }
    }
}