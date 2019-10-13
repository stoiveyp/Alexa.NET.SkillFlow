namespace Alexa.NET.SkillFlow
{
    public static class SkillFlowComponentExtensions
    {
        public static InvalidSkillFlowException InvalidComponent(this SkillFlowComponent component,
            SkillFlowComponent candidate) =>
            new InvalidSkillFlowException($"Unable to add {candidate.Type} to {component.Type}");
    }
}