using System;
using System.Collections.Generic;
using System.Text;

namespace Alexa.NET.SkillFlow
{
    public class SceneInstructions : SceneInstructionContainer,ISkillFlowComponent
    {
        public List<ISceneInstruction> Instructions { get; set; } = new List<ISceneInstruction>();

        public override bool Group => false;
        public override string Type => nameof(SceneInstructions);
        public void Add(ISkillFlowComponent component)
        {
            if (component is ISceneInstruction instruction)
            {
                Instructions.Add(instruction);
            }
            else
            {
                throw this.InvalidComponent(component);
            }
        }
    }
}
