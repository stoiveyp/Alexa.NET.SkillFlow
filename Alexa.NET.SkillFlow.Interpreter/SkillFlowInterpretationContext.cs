using System;
using System.Collections.Generic;

namespace Alexa.NET.SkillFlow.Interpreter
{
    public class SkillFlowInterpretationContext
    {
        public SkillFlowInterpretationContext(SkillFlowInterpretationOptions options)
        {
            Options = options;
            Components = new Stack<SkillFlowComponent>();
            Story = new Story();
            Components.Push(Story);
        }

        public List<string> Comments { get; } = new List<string>();

        public Story Story { get; }

        public SkillFlowInterpretationOptions Options { get; }
        public Stack<SkillFlowComponent> Components { get; }

        public int LineNumber { get; set; }

        public SkillFlowComponent CurrentComponent => Components.Peek();
    }
}