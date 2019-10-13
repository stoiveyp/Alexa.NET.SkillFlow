using System;
using System.Collections.Generic;
using System.Text;

namespace Alexa.NET.SkillFlow
{
    public class Scene : SkillFlowComponent
    {
        public Scene() { }

        public Scene(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
        public override string Type => nameof(Scene);
        public override void Add(SkillFlowComponent component)
        {
            if (component is Text text)
            {
                switch (text.TextType)
                {
                    case "say":
                        this.Say = text;
                        break;
                    case "reprompt":
                        this.Reprompt = text;
                        break;
                    case "recap":
                        this.Recap = text;
                        break;
                }
            }
            else if (component is Visual visual)
            {
                this.Visual = visual;
            }
            else if (component is SceneInstructions instructions)
            {
                this.Instructions = instructions;
            }
            else
            {
                throw this.InvalidComponent(component);
            }
        }

        public Text Say { get; protected set; }
        public Text Reprompt { get; protected set; }
        public Text Recap { get; protected set; }
        public Visual Visual { get; protected set; }
        public SceneInstructions Instructions { get; protected set; }
    }
}
