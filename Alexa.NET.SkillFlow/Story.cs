using System;
using System.Collections.Generic;
using System.Text;

namespace Alexa.NET.SkillFlow
{
    public class Story:ISkillFlowComponent
    {
        public Dictionary<string,Scene> Scenes { get; set; } = new Dictionary<string, Scene>();
        public string Type => "Story";

        public void Add(ISkillFlowComponent component)
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
