namespace Alexa.NET.SkillFlow.Terminators
{
    public class Restart:ISceneTerminator
    {
        public string Type => nameof(Restart);
        public void Add(ISkillFlowComponent component)
        {
            throw this.InvalidComponent(component);
        }
    }
}