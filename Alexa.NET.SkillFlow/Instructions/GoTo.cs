using System;
using System.Collections.Generic;
using System.Text;

namespace Alexa.NET.SkillFlow.Instructions
{
    public class GoTo:SceneInstruction
    {
        public GoTo() { }

        public GoTo(string sceneName)
        {
            SceneName = sceneName;
        }
        public string SceneName { get; set; }
        public override string Type => nameof(GoTo);
    }
}
