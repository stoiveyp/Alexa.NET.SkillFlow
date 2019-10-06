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
                        context.Push(new OpenGroup());
                        context.MoveNext();
                        continue;
                    case ')':
                        context.Push(new CloseGroup());
                        context.MoveNext();
                        continue;
                    default:
                        if (!ProcessWord(context))
                        {
                            context.MoveCurrent();
                        }
                        continue;
                }
            }

            if (context.CurrentWord.Length > 0)
            {
                context.Push(new LiteralValue(context.CurrentWord));
                context.MoveCurrent();
                context.MoveToCurrent();
                
            }
        }

        private static bool ProcessWord(ConditionContext context)
        {
            return false;
        }
    }
}
