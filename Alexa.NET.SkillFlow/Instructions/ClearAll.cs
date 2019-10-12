using System;
using System.Collections.Generic;
using System.Text;

namespace Alexa.NET.SkillFlow.Instructions
{
    public class ClearAll:ISkillFlowComponent
    {
        public string Type => nameof(ClearAll);
        public void Add(ISkillFlowComponent component)
        {
            throw this.InvalidComponent(component);
        }
    }
}
