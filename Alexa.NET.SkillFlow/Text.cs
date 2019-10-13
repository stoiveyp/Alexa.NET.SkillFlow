using System;
using System.Collections.Generic;
using System.Text;

namespace Alexa.NET.SkillFlow
{
    public class Text:SkillFlowComponent
    {
        public Text(string type)
        {
            TextType = type;
        }

        public override string Type => nameof(Text);

        public string TextType { get; }

        public List<string> Content { get; set; } = new List<string>();
        public override void Add(SkillFlowComponent component)
        {
            if (component is Variation _)
            {
                Content.Add(string.Empty);
                return;
            }
            else if (component is TextLine textline)
            {
                if (Content.Count == 0)
                {
                    Content.Add(textline.Text);
                }
                else
                {
                    var oldContent = Content[Content.Count - 1];
                    Content.RemoveAt(Content.Count-1);
                    Content.Add(oldContent + textline.Text);
                }

                return;
            }
            throw this.InvalidComponent(component);
        }
    }
}
