using System;
using System.Collections.Generic;
using System.Text;

namespace Alexa.NET.SkillFlow
{
    public class Text:ISkillFlowComponent
    {
        public Text() { }

        public Text(string content)
        {
            Content = content;
        }

        public string Type => nameof(Text);

        public string Content { get; set; }
        public void Add(ISkillFlowComponent component)
        {
            throw this.InvalidComponent(component);
        }
    }
}
