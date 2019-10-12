using System;
using System.Collections.Generic;
using System.Text;

namespace Alexa.NET.SkillFlow.Terminators
{
    public class Resume : ISceneTerminator
    {
        public string Type => nameof(Resume);
        public void Add(ISkillFlowComponent component)
        {
            throw this.InvalidComponent(component);
        }
    }
}
