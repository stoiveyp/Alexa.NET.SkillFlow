using System;
using System.Collections.Generic;
using System.Text;

namespace Alexa.NET.SkillFlow.Instructions
{
    public class GoTo:ISceneInstruction
    {
        public GoTo() { }

        public GoTo(string sceneName)
        {
            SceneName = sceneName;
        }
        public string SceneName { get; set; }
        public string Type => nameof(GoTo);
        public void Add(ISkillFlowComponent component)
        {
            throw this.InvalidComponent(component);
        }
    }
}
