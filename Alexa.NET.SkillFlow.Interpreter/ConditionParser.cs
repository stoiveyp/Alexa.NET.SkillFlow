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
                if (context.CurrentWord.Length == 0)
                {
                    switch (context.NextChar)
                    {
                        case '(':
                            context.Push(new OpenGroup());
                            context.MoveNext();
                            continue;
                        case ')':
                            context.Push(new CloseGroup());
                            context.MoveNext();
                            continue;
                        case '=':
                            if (context.Peek.HasValue && context.Peek.Value == '=')
                            {
                                context.Push(new Equal());
                                context.MoveNext(2);
                                continue;
                            }

                            break;
                        case '!':
                            if (context.Peek.HasValue && context.Peek.Value == '=')
                            {
                                context.Push(new NotEqual());
                                context.MoveNext(2);
                                continue;
                            }
                            context.Push(new Not());
                            context.MoveNext();
                            continue;
                        case '<':
                            if (context.Peek.HasValue && context.Peek.Value == '=')
                            {
                                context.Push(new LessThanEqual());
                                context.MoveNext(2);
                                continue;
                            }
                            context.Push(new LessThan());
                            context.MoveNext();
                            continue;
                        case '>':
                            if (context.Peek.HasValue && context.Peek.Value == '=')
                            {
                                context.Push(new GreaterThanEqual());
                                context.MoveNext(2);
                                continue;
                            }
                            context.Push(new GreaterThan());
                            context.MoveNext();
                            continue;
                        case '&':
                            if (context.Peek == '&')
                            {
                                context.Push(new And());
                                context.MoveNext(2);
                                continue;
                            }

                            break;
                        case '|':
                            if (context.Peek == '|')
                            {
                                context.Push(new Or());
                                context.MoveNext(2);
                                continue;
                            }

                            break;
                    }
                    context.MoveCurrent();
                }
                else
                {
                    ProcessWord(context);
                    context.MoveCurrent();
                }
            }

            if (context.CurrentWord.Length > 0)
            {
                context.Push(new LiteralValue(context.CurrentWord));
            }
        }

        private static void ProcessWord(ConditionContext context)
        {
            switch (context.CurrentWord)
            {
                case " or ":
                    context.Push(new Or());
                    context.MoveToCurrent();
                    return;
                case " and ":
                    context.Push(new And());
                    context.MoveToCurrent();
                    return;
                case " is ":
                    context.MoveToCurrent();
                    if (context.PeekWord("less than "))
                    {
                        context.MoveNext(10);
                        if (context.PeekWord("or equal "))
                        {
                            context.MoveNext(9);
                            context.Push(new LessThanEqual());
                        }
                        else
                        {
                            context.Push(new LessThan());
                        }
                    }
                    else if (context.PeekWord("greater than "))
                    {
                        context.MoveNext(13);
                        if (context.PeekWord("or equal "))
                        {
                            context.MoveNext(9);
                            context.Push(new GreaterThanEqual());
                        }
                        else
                        {
                            context.Push(new GreaterThan());
                        }
                    }
                    else if (context.PeekWord("not "))
                    {
                        context.MoveNext(4);
                        context.Push(new NotEqual());
                    }
                    else
                    {
                        context.Push(new Equal());
                    }
                    return;
            }
        }
    }
}
