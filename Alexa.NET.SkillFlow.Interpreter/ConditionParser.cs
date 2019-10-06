using System;
using Alexa.NET.SkillFlow.Conditions;

namespace Alexa.NET.SkillFlow.Interpreter
{
    public class ConditionParser
    {
        public static Condition Parse(string condition)
        {
            var context = new ConditionContext(condition);

            if (string.IsNullOrWhiteSpace(condition))
            {
                return context.Condition;
            }

            if (!context.Finished)
            {
                throw new InvalidConditionException(condition);
            }

            return context.Condition;
        }
    }
}
