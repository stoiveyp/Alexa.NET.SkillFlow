using System;
using System.Collections.Generic;
using System.Text;

namespace Alexa.NET.SkillFlow.Instructions
{
    public class SlotAssignment:SceneInstruction
    {
        public SlotAssignment()
        {

        }

        public SlotAssignment(string slotName, string slotType)
        {
            SlotName = slotName;
            SlotType = slotType;
        }

        public string SlotType { get; set; }

        public string SlotName { get; set; }
        public override string Type => nameof(SlotAssignment);
    }
}
