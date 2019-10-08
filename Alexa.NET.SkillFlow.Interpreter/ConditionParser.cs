using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
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

            return Stack(context.Values, condition);
        }

        public static readonly Type[] StackPrecedence =
        {
            typeof(LiteralValue),
            typeof(Equal),
            typeof(OpenGroup),
            typeof(CloseGroup)
        };

        private static bool HigherPrecedence(Value candidate, Value current)
        {
            return Array.IndexOf(StackPrecedence, candidate.GetType()) >
                   (current == null
                       ? -1
                       : Array.IndexOf(StackPrecedence, current.GetType()));
        }

        private static Value SafePeek(Stack<Value> stack)
        {
            return (stack.Any() ? stack.Peek() : null);
        }

        public static Condition Stack(Stack<Value> stack, string condition)
        {
            if (stack.Count < 2)
            {
                return MakeCondition(stack, condition);
            }

            var arrange = new Stack<Value>();

            while (SafePeek(stack) != null)
            {
                var token = stack.Pop();
                if (HigherPrecedence(token, SafePeek(arrange)))
                {
                    arrange.Push(token);
                }
                else
                {
                    var subStack = new Stack<Value>(new[] { token });
                    while (!HigherPrecedence(SafePeek(arrange), token))
                    {
                        subStack.Push(arrange.Pop());
                    }

                    var side = Stack(subStack, condition);
                    var current = arrange.Pop();
                    if (current is BinaryCondition binary)
                    {
                        binary.Left = side;
                        binary.Right = Stack(arrange, condition);
                        return binary;
                    }

                    return side;
                }
            }

            if (arrange.Count > 1)
            {
                throw new InvalidConditionException(condition);
            }

            return MakeCondition(arrange, condition);
        }

        private static Condition MakeCondition(Stack<Value> stack, string originalCondition)
        {
            if (stack.Count > 1 || stack.First() == null)
            {
                throw new InvalidConditionException(originalCondition);
            }

            var final = stack.Pop();
            return final is Condition finalCondition ? finalCondition : new ValueWrapper(final);
        }

        public static void Tokenise(ConditionContext context)
        {
            while (!context.Finished)
            {
                while (context.NextChar.HasValue && context.NextChar == ' ')
                {
                    context.MoveNext();
                }

                if (!context.NextChar.HasValue)
                {
                    context.MoveNext();
                    break;
                }

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
                    default:
                        ProcessWord(context);
                        break;
                }
            }

            if (context.CurrentWord.Length > 0)
            {
                context.Push(new LiteralValue(context.CurrentWord));
            }
        }

        private static void ProcessWord(ConditionContext context)
        {
            MoveToBreaker(context);

            switch (context.CurrentWord)
            {
                case "or":
                    context.Push(new Or());
                    context.MoveToCurrent();
                    return;
                case "and":
                    context.Push(new And());
                    context.MoveToCurrent();
                    return;
                case "is":
                    context.MoveToCurrent();
                    if (context.PeekWord(" less than "))
                    {
                        context.MoveNext(11);
                        if (context.PeekWord("or equal "))
                        {
                            context.MoveNext(9);
                            context.Push(new LessThanEqual());
                        }
                        else
                        {
                            context.Push(new LessThan());
                        }

                        return;
                    }
                    else if (context.PeekWord(" greater than "))
                    {
                        context.MoveNext(14);
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
                    else if (context.PeekWord(" not "))
                    {
                        context.MoveNext(5);
                        context.Push(new NotEqual());
                    }
                    else
                    {
                        context.Push(new Equal());
                    }
                    return;
                default:
                    if (context.CurrentWord.Trim().Length > 0)
                    {
                        context.Push(new LiteralValue(context.CurrentWord.Trim()));
                        context.MoveToCurrent();
                    }
                    break;
            }
        }

        static readonly char?[] breakers = { '(', ')', ' ', '<', '>', '=', '!', '|', '&' };
        private static void MoveToBreaker(ConditionContext context)
        {
            while (context.PeekCurrent.HasValue && !breakers.Contains(context.PeekCurrent))
            {
                context.MoveCurrent();
            }
            context.MoveCurrent();
        }
    }
}
