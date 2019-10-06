using System;
using System.Collections.Generic;
using System.Text;

namespace Alexa.NET.SkillFlow.Interpreter
{
    public struct InterpreterResult
    {
        public InterpreterResult(int used, ISkillFlowComponent component)
        {
            Used = used;
            Component = component;
        }

        public int Used { get; set; }
        public ISkillFlowComponent Component { get; set; }

        public static InterpreterResult Empty => new InterpreterResult(0,null);
    }
}
