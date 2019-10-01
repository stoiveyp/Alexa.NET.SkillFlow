using System;
using System.Collections.Generic;
using System.Text;

namespace Alexa.NET.SkillFlow
{
    public class TextLine:ISkillFlowComponent
    {
        public TextLine(string text)
        {
            Text = text;
        }

        public string Text { get; set; }

        public string Type => nameof(TextLine);
        public void Add(ISkillFlowComponent component)
        {
            throw new NotImplementedException();
        }
    }
}
