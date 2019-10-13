using System;
using System.Collections.Generic;
using System.Text;

namespace Alexa.NET.SkillFlow
{
    public abstract class SceneInstructionContainer:SceneInstruction
    {
        public List<SceneInstruction> Instructions { get; set; } = new List<SceneInstruction>();

        public override void Add(SkillFlowComponent component)
        {
            if (component is SceneInstruction instruction)
            {
                Instructions.Add(instruction);
            }
            else
            {
                throw new InvalidSkillFlowException($"Unable to add {component.Type} to {Type}");
            }
        }
    }
}
