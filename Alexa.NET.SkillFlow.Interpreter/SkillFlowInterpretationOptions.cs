using System;
using System.Collections.Generic;
using System.Text;

namespace Alexa.NET.SkillFlow.Interpreter
{
    public class SkillFlowInterpretationOptions
    {
        public string LineEnding { get; set; }

        public SkillFlowInterpretationOptions()
        {
            LineEnding = Environment.NewLine;
        }
    }
}
