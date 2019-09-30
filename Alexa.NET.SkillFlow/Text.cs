using System;
using System.Collections.Generic;
using System.Text;

namespace Alexa.NET.SkillFlow
{
    public class Text:ISkillFlowComponent
    {
        public Text(string type)
        {
            TextType = type;
        }

        public string Type => nameof(Text);

        public string TextType { get; }

        public List<string> Content { get; set; } = new List<string>();
        public void Add(ISkillFlowComponent component)
        {
            throw this.InvalidComponent(component);
        }
    }
}
