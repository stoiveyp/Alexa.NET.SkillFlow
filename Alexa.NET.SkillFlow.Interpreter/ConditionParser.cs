using System;
using System.Collections.Generic;
using System.Linq;
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
            typeof(GreaterThan),
            typeof(GreaterThanEqual),
            typeof(LessThan),
            typeof(LessThanEqual),
            typeof(Not),
            typeof(Equal),
            typeof(And),
            typeof(Or),
            typeof(OpenGroup),
            typeof(CloseGroup)
        };

        private static int Precedence(Value candidate)
        {
            return candidate == null
                       ? -1
                       : Array.IndexOf(StackPrecedence, candidate.GetType());
        }

        private static Value SafePeek(Stack<Value> stack)
        {
            return (stack.Any() ? stack.Peek() : null);
        }

        public static Condition Stack(Stack<Value> stack, string condition)
        {
            if (stack.Count < 2)
            {
                return MakeCondition(stack.First());
            }

            var priorityTokens = new Stack<Value>();
            var ops = new Stack<Value>();

            while (SafePeek(stack) != null)
            {
                var token = stack.Pop();
                if (token is CloseGroup)
                {
                    token = new Group(Stack(stack, condition));
                }

                if (token is OpenGroup)
                {
                    while (ops.Any())
                    {
                        priorityTokens.Push(ops.Pop());
                    }
                    return MakeCondition(HandleToken(priorityTokens.Pop(), priorityTokens));
                }

                if (Precedence(token) <= Precedence(SafePeek(priorityTokens)))
                {
                    priorityTokens.Push(token);
                }
                else if (ops.Any() && Precedence(token) > Precedence(ops.Peek()))
                {
                    var currentOp = ops.Pop();
                    priorityTokens.Push(HandleToken(currentOp, priorityTokens));
                    ops.Push(token);
                }
                else
                {
                    ops.Push(token);
                }
            }

            while (ops.Any())
            {
                priorityTokens.Push(ops.Pop());
            }

            return MakeCondition(HandleToken(priorityTokens.Pop(),priorityTokens));
        }

        private static Value HandleToken(Value op, Stack<Value> stack)
        {
            if (stack.Any() && op is BinaryCondition binary)
            {
                if (binary.Left == null)
                {
                    binary.Left = HandleToken(stack.Pop(), stack);
                }

                if (binary.Right == null)
                {
                    binary.Right = HandleToken(stack.Pop(), stack);
                }

                return binary;
            }
            else if (stack.Any() && op is UnaryCondition unary)
            {
                if (unary.Condition == null)
                {
                    unary.Condition = HandleToken(stack.Pop(), stack);
                }
            }

            return op;
        }

        private static Condition MakeCondition(Value final)
        {
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
