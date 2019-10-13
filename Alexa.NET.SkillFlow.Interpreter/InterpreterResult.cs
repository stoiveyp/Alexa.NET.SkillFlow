using System;
using System.Collections.Generic;
using System.Text;

namespace Alexa.NET.SkillFlow.Interpreter
{
    public struct InterpreterResult
    {
        public InterpreterResult(SkillFlowComponent component)
        {
            Component = component;
        }

        public SkillFlowComponent Component { get; set; }

        public static InterpreterResult Empty => new InterpreterResult(null);
    }
}
