﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Alexa.NET.SkillFlow
{
    public class Scene:ISkillFlowComponent
    {
        public Scene() { }

        public Scene(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
        public string Type => nameof(Scene);
        public void Add(ISkillFlowComponent component)
        {
            throw this.InvalidComponent(component);
        }
    }
}
