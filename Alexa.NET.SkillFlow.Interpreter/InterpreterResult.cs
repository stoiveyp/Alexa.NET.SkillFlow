using System;
using System.Collections.Generic;
using System.Text;

namespace Alexa.NET.SkillFlow.Interpreter
{
    public struct InterpreterResult
    {
        public InterpreterResult(ISkillFlowComponent component)
        {
            Component = component;
        }

        public ISkillFlowComponent Component { get; set; }

        public static InterpreterResult Empty => new InterpreterResult(null);

        public static bool IsEmpty(InterpreterResult result)
        {
            return result.Component == null;
        }
    }
}
