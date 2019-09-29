namespace Alexa.NET.SkillFlow
{
    public static class SkillFlowComponentExtensions
    {
        public static InvalidSkillFlowException InvalidComponent(this ISkillFlowComponent component,
            ISkillFlowComponent candidate) =>
            new InvalidSkillFlowException($"Unable to add {candidate.Type} to {component.Type}");
    }
}