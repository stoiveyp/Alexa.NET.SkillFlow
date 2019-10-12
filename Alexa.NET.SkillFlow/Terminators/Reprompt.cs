namespace Alexa.NET.SkillFlow.Terminators
{
    public class Reprompt : ISceneTerminator
    {
        public string Type => nameof(Reprompt);
        public void Add(ISkillFlowComponent component)
        {
            throw this.InvalidComponent(component);
        }
    }
}