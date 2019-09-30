using System;
using System.Collections.Generic;
using System.Text;

namespace Alexa.NET.SkillFlow
{
    public class Variation:ISkillFlowComponent
    {
        public string Type => nameof(Variation);
        public void Add(ISkillFlowComponent component)
        {
            throw new NotImplementedException();
        }
    }
}
