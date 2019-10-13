using System;
using System.Collections.Generic;
using System.Text;

namespace Alexa.NET.SkillFlow
{
    public abstract class SkillFlowComponent
    {
        public string[] Comments { get; set; }
        public abstract string Type { get; }

        public virtual void Add(SkillFlowComponent component)
        {
            throw this.InvalidComponent(component);
        }
    }
}
