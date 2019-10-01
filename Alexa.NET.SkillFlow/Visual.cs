using System;
using System.Collections.Generic;
using System.Text;

namespace Alexa.NET.SkillFlow
{
    public class Visual:ISkillFlowComponent
    {
        public string Type => nameof(Visual);
        public void Add(ISkillFlowComponent component)
        {
            throw new NotImplementedException();
        }
    }
}
