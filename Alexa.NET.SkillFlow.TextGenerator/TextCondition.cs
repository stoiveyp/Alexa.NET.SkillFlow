using System;
using System.Threading.Tasks;
using Alexa.NET.SkillFlow.Conditions;

namespace Alexa.NET.SkillFlow.TextGenerator
{
    public class TextCondition
    {
        public static async Task Render(Value condition, TextGeneratorContext context)
        {
            if (condition is BinaryCondition binary)
            {
                await Render(binary.Left,context);
                await RenderValue(binary,context);
                await Render(binary.Right, context);
            }
            else if (condition is UnaryCondition unary)
            {
                await RenderValue(unary, context);
                await Render(unary.Condition, context);
                await RenderValue(unary, context,false);
            }
            else
            {
                await RenderValue(condition, context);
            }
        }

        private static Task RenderValue(Value value, TextGeneratorContext context, bool start = true)
        {
            switch (value)
            {
                case And and:
                    return context.WriteString(" and ");
                case Equal equal:
                    return context.WriteString(" == ");
                case False falseValue:
                    return context.WriteString("false");
                case GreaterThan greaterThan:
                    return context.WriteString(" > ");
                case GreaterThanEqual greaterThanEqual:
                    return context.WriteString(" >= ");
                case LessThan lessThan:
                    return context.WriteString(" < ");
                case LessThanEqual lessThanEqual:
                    return context.WriteString(" <= ");
                case LiteralValue literalValue:
                    return context.WriteString(literalValue.Value.ToString());
                case Not not:
                    if (start)
                    {
                        return context.WriteString("!");
                    }

                    return Task.FromResult(context);
                case NotEqual notEqual:
                    return context.WriteString(" != ");
                case Or or:
                    return context.WriteString(" or ");
                case True trueValue:
                    return context.WriteString("true");
                case ValueWrapper valueWrapper:
                    return RenderValue(valueWrapper.Value,context);
                case Variable variable:
                    return context.WriteString(variable.Name);
                case Group group:
                    return context.WriteString(start ? "( " : " )");
                default:
                    throw new ArgumentOutOfRangeException(nameof(value));
            }
        }
    }
}