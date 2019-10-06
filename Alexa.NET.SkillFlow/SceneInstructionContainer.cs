using System;
using System.Collections.Generic;
using System.Text;

namespace Alexa.NET.SkillFlow
{
    public abstract class SceneInstructionContainer
    {
        public abstract string Type { get; }
        public List<ISceneInstruction> Instructions { get; set; } = new List<ISceneInstruction>();
        public void Add(ISkillFlowComponent component)
        {
            if (component is ISceneInstruction instruction)
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
