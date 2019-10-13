using System;
using System.Collections.Generic;
using System.Text;

namespace Alexa.NET.SkillFlow.Instructions
{
    public class Hear : SceneInstructionContainer
    {
        public Hear()
        {
            Phrases = new List<string>();
        }

        public Hear(IEnumerable<string> phrases)
        {
            Phrases = new List<string>(phrases);
        }

        public Hear(params string[] phrases)
        {
            Phrases = new List<string>(phrases);
        }

        public override bool Group => true;
        public List<string> Phrases { get; set; }
        public override string Type => nameof(Hear);
    }
}
