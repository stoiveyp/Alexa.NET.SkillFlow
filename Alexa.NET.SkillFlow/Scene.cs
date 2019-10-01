using System;
using System.Collections.Generic;
using System.Text;

namespace Alexa.NET.SkillFlow
{
    public class Scene:ISkillFlowComponent
    {
        public Scene() { }

        public Scene(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
        public string Type => nameof(Scene);
        public void Add(ISkillFlowComponent component)
        {
            if (component is Text text)
            {
                switch (text.TextType)
                {
                    case "say":
                        this.Say = text;
                        return;
                    case "reprompt":
                        this.Reprompt = text;
                        return;
                    case "recap":
                        this.Recap = text;
                        return;
                }
            }
            throw this.InvalidComponent(component);
        }

        public Text Say { get; set; }
        public Text Reprompt { get; set; }
        public Text Recap { get; set; }
    }
}
