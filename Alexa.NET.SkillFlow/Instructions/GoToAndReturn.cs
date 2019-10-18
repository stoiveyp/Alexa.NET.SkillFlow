using System;
using System.Collections.Generic;
using System.Text;

namespace Alexa.NET.SkillFlow.Instructions
{
    public class GoToAndReturn : SceneInstruction
    {
        public GoToAndReturn() { }

        public GoToAndReturn(string sceneName)
        {
            SceneName = sceneName;
        }
        public override string Type => nameof(GoToAndReturn);
        public string SceneName { get; set; }
    }
}