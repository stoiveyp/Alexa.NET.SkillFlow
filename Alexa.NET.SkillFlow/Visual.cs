using System;
using System.Collections.Generic;
using System.Text;

namespace Alexa.NET.SkillFlow
{
    public class Visual:ISkillFlowComponent
    {
        public string Type => nameof(Visual);

        private VisualProperty _template;
        private VisualProperty _background;
        private VisualProperty _title;
        private VisualProperty _subtitle;
        public string Template => _template.Value;
        public string Background => _background.Value;
        public string Title => _title.Value;
        public string Subtitle => _subtitle.Value;

        public void Add(ISkillFlowComponent component)
        {
            switch (component)
            {
                case VisualProperty property:
                    switch (property.Key)
                    {
                        case "template":
                            _template = property;
                            break;
                        case "background":
                            _background = property;
                            break;
                        case "title":
                            _title = property;
                            break;
                        case "subtitle":
                            _subtitle = property;
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
