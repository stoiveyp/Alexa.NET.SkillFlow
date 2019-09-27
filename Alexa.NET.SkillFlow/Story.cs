using System;
using System.Collections.Generic;
using System.Text;

namespace Alexa.NET.SkillFlow
{
    public class Story
    {
        public Dictionary<string,Scene> Scenes { get; set; } = new Dictionary<string, Scene>();
    }
}
