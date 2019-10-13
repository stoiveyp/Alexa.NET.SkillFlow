using System;
using System.Collections.Generic;
using System.Text;

namespace Alexa.NET.SkillFlow
{
    public class Story:SkillFlowComponent
    {
        public Dictionary<string,Scene> Scenes { get; set; } = new Dictionary<string, Scene>();
        public override string Type => nameof(Story);

        public override void Add(SkillFlowComponent component)
        {
            switch (component)
            {
                case Scene scene:
                    Scenes.Add(scene.Name, scene);
                    break;
                default:
                    throw this.InvalidComponent(component);
            }
        }
    }
}
