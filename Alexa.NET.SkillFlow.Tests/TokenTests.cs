using System.Linq;
using Alexa.NET.SkillFlow.Conditions;
using Alexa.NET.SkillFlow.Interpreter;
using Xunit;

namespace Alexa.NET.SkillFlow.Tests
{
    public class TokenTests
    {
        [Fact]
        public void InvalidConditionReturnsWrapper()
        {
            var condition = ConditionParser.Parse("~");
            var wrapper = Assert.IsType<ValueWrapper>(condition);
            var literal = Assert.IsType<LiteralValue>(wrapper.Value);
            Assert.Equal("~",literal.Value);
        }

        [Fact]
        public void EmptyConditionEqualsFalse()
        {
            var condition = ConditionParser.Parse(string.Empty);
            var wrapper = Assert.IsType<ValueWrapper>(condition);
            Assert.IsType<False>(wrapper.Value);
        }

        [Fact]
        public void Groups()
        {
            var context = new ConditionContext("()");
            ConditionParser.Tokenise(context);
            Assert.Equal(2,context.Values.Count);
        }

        [Fact]
        public void NotWord()
        {
            var context = new ConditionContext("!");
            ConditionParser.Tokenise(context);
            Assert.IsType<Not>(context.Values.First());
        }

        [Fact]
        public void OrWord()
        {
            var context = new ConditionContext(" or ");
            ConditionParser.Tokenise(context);
            var value = Assert.Single(context.Values);
            Assert.IsType<Or>(value);
        }

        [Fact]
        public void OrSymbol()
        {
            var context = new ConditionContext("||");
            ConditionParser.Tokenise(context);
            var value = Assert.Single(context.Values);
            Assert.IsType<Or>(value);
        }

        [Fact]
        public void AndSymbol()
        {
            var context = new ConditionContext("&&");
            ConditionParser.Tokenise(context);
            var value = Assert.Single(context.Values);
            Assert.IsType<And>(value);
        }

        [Fact]
        public void AndWord()
        {
            var context = new ConditionContext(" and ");
            ConditionParser.Tokenise(context);
            var value = Assert.Single(context.Values);
            Assert.IsType<And>(value);
        }

        [Fact]
        public void EqualSymbol()
        {
            var context = new ConditionContext("==");
            ConditionParser.Tokenise(context);
            var value = Assert.Single(context.Values);
            Assert.IsType<Equal>(value);
        }

        [Fact]
        public void EqualWord()
        {
            var context = new ConditionContext(" is ");
            ConditionParser.Tokenise(context);
            var value = Assert.Single(context.Values);
            Assert.IsType<Equal>(value);
        }

        [Fact]
        public void NotEqualSymbol()
        {
            var context = new ConditionContext("!=");
            ConditionParser.Tokenise(context);
            var value = Assert.Single(context.Values);
            Assert.IsType<NotEqual>(value);
        }

        [Fact]
        public void NotEqualWord()
        {
            var context = new ConditionContext(" is not ");
            ConditionParser.Tokenise(context);
            var value = Assert.Single(context.Values);
            Assert.IsType<NotEqual>(value);
        }

        [Fact]
        public void LessThanEqualSymbol()
        {
            var context = new ConditionContext("<=");
            ConditionParser.Tokenise(context);
            var value = Assert.Single(context.Values);
            Assert.IsType<LessThanEqual>(value);
        }

        [Fact]
        public void LessThanEqualWord()
        {
            var context = new ConditionContext(" is less than or equal ");
            ConditionParser.Tokenise(context);
            var value = Assert.Single(context.Values);
            Assert.IsType<LessThanEqual>(value);
        }

        [Fact]
        public void LessThanSymbol()
        {
            var context = new ConditionContext("<");
            ConditionParser.Tokenise(context);
            var value = Assert.Single(context.Values);
            Assert.IsType<LessThan>(value);
        }

        [Fact]
        public void LessThanWord()
        {
            var context = new ConditionContext(" is less than ");
            ConditionParser.Tokenise(context);
            var value = Assert.Single(context.Values);
            Assert.IsType<LessThan>(value);
        }

        [Fact]
        public void GreaterThanEqualSymbol()
        {
            var context = new ConditionContext(">=");
            ConditionParser.Tokenise(context);
            var value = Assert.Single(context.Values);
            Assert.IsType<GreaterThanEqual>(value);
        }

        [Fact]
        public void GreaterThanEqualWord()
        {
            var context = new ConditionContext(" is greater than or equal ");
            ConditionParser.Tokenise(context);
            var value = Assert.Single(context.Values);
            Assert.IsType<GreaterThanEqual>(value);
        }

        [Fact]
        public void GreaterThanSymbol()
        {
            var context = new ConditionContext(">");
            ConditionParser.Tokenise(context);
            var value = Assert.Single(context.Values);
            Assert.IsType<GreaterThan>(value);
        }

        [Fact]
        public void GreaterThanWord()
        {
            var context = new ConditionContext(" is greater than ");
            ConditionParser.Tokenise(context);
            var value = Assert.Single(context.Values);
            Assert.IsType<GreaterThan>(value);
        }
    }
}
