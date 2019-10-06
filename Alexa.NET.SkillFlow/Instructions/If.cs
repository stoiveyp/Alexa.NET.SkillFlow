using System;
using System.Collections.Generic;
using System.Text;
using Alexa.NET.SkillFlow.Conditions;

namespace Alexa.NET.SkillFlow.Instructions
{
    public class If: SceneInstructionContainer,ISceneInstruction
    {
        public If() { }

        public If(Condition condition)
        {
            Condition = condition;
        }
        public override string Type => nameof(If);

        public Condition Condition { get; set; }
    }
}
