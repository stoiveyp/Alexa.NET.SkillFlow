namespace Alexa.NET.SkillFlow.Terminators
{
    public class Return : ISceneTerminator
    {
        public string Type => nameof(Return);
        public void Add(ISkillFlowComponent component)
        {
            throw this.InvalidComponent(component);
        }
    }
}