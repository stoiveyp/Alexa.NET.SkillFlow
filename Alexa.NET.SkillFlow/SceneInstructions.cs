using System;
using System.Collections.Generic;
using System.Text;

namespace Alexa.NET.SkillFlow
{
    public class SceneInstructions:ISkillFlowComponent
    {
        public List<ISceneInstruction> Instructions { get; set; } = new List<ISceneInstruction>();
        public string Type => nameof(SceneInstructions);
        public void Add(ISkillFlowComponent component)
        {
            throw this.InvalidComponent(component);
        }
    }
}
