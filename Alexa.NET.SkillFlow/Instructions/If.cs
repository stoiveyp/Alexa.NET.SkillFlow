using System;
using System.Collections.Generic;
using System.Text;

namespace Alexa.NET.SkillFlow.Instructions
{
    public class If: SceneInstructionContainer,ISceneInstruction
    {
        public override string Type => nameof(If);
    }
}
