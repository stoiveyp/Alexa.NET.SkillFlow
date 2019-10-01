using System;
using System.Collections.Generic;
using System.Text;

namespace Alexa.NET.SkillFlow
{
    public class InvalidSkillFlowException:Exception
    {
        public InvalidSkillFlowException(string message):base(message)
        {
            
        }
    }
}
