using System;
using System.Diagnostics;
using Alexa.NET.SkillFlow.Conditions;
using Alexa.NET.SkillFlow.Interpreter.Tokens;

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

            Tokenise(context);

            if (!context.Finished)
            {
                throw new InvalidConditionException(condition);
            }

            return context.Condition;
        }

        public static void Tokenise(ConditionContext context)
        {
            while (!context.Finished)
            {
                switch (context.CurrentChar)
                {
                    case '(':
                        context.Values.Push(new OpenGroup());
                        context.MoveNext();
                        continue;
                    case ')':
                        context.Values.Push(new CloseGroup());
                        context.MoveNext();
                        continue;
                }

                break;
            }
        }
    }
}
