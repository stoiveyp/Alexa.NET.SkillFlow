using System;
using System.Collections.Generic;

namespace Alexa.NET.SkillFlow.Interpreter
{
    public class SkillFlowInterpretationContext
    {
        public SkillFlowInterpretationContext(SkillFlowInterpretationOptions options)
        {
            Options = options;
            Components = new Stack<ISkillFlowComponent>();
            Story = new Story();
            Components.Push(Story);
        }

        public Story Story { get; }

        public SkillFlowInterpretationOptions Options { get; }
        public Stack<ISkillFlowComponent> Components { get; }
        public int LineNumber { get; set; }
        public int InterpretAttempts { get; set; }
        public ISkillFlowComponent CurrentComponent => Components.Peek();
    }
}