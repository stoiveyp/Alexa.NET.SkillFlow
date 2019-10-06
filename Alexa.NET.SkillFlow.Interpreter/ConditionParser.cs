using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

    public class ConditionContext
    {
        public StringBuilder Remaining { get; set; }
        public Stack<Value> Values { get; set; }

        public Condition Condition
        {
            get
            {
                if (Values.Count == 0)
                {
                    return new ValueWrapper(new False());
                }

                if (Values.First() is Condition)
                {
                    return Values.First() as Condition;
                }

                return new ValueWrapper(Values.First());
            }
        }

        public bool Finished => Remaining.Length == 0;

        public ConditionContext(string condition)
        {
            Remaining = new StringBuilder(condition);
            Values = new Stack<Value>();
        }
    }
}
