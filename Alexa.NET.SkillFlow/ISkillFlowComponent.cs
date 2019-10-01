using System;
using System.Collections.Generic;
using System.Text;

namespace Alexa.NET.SkillFlow
{
    public interface ISkillFlowComponent
    {
        string Type { get; }
        void Add(ISkillFlowComponent component);
    }
}
