using System;
using System.Collections.Generic;
using System.Text;

namespace Alexa.NET.SkillFlow
{
    public class Visual : SkillFlowComponent
    {
        public override string Type => nameof(Visual);

        public VisualProperty Template { get; set; }
        public VisualProperty Background { get; set; }

        public VisualProperty Title { get; set; }

        public VisualProperty Subtitle { get; set; }

        public override void Add(SkillFlowComponent component)
        {
            switch (component)
            {
                case VisualProperty property:
                    switch (property.Key)
                    {
                        case "template":
                            Template = property;
                            break;
                        case "background":
                            Background = property;
                            break;
                        case "title":
                            Title = property;
                            break;
                        case "subtitle":
                            Subtitle = property;
                            break;
                        default:
                            throw this.InvalidComponent(component);
                    }
                    break;
                default:
                    throw this.InvalidComponent(component);
            }
        }
    }
}
