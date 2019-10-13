using System;
using System.Collections.Generic;
using System.Text;

namespace Alexa.NET.SkillFlow
{
    public class TextLine:SkillFlowComponent
    {
        public TextLine(string text)
        {
            Text = text;
        }

        public string Text { get; set; }

        public override string Type => nameof(TextLine);

    }
}
