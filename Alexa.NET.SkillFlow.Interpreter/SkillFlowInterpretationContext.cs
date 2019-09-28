using System;

namespace Alexa.NET.SkillFlow.Interpreter
{
    public class SkillFlowInterpretationContext
    {
        public SkillFlowInterpretationContext(SkillFlowInterpretationOptions options)
        {
            Options = options;
            Story = new Story();
        }
        public SkillFlowInterpretationOptions Options { get; }
        public Story Story { get; }
        public int LineNumber { get; set; }
        public int InterpretAttempts { get; set; }
    }
}