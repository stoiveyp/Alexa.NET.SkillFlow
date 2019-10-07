using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alexa.NET.SkillFlow.Conditions;
using Alexa.NET.SkillFlow.Interpreter;
using Xunit;

namespace Alexa.NET.SkillFlow.Tests
{
    public class ConditionTests
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
        public void NotEqualWord()
        {
            var context = new ConditionContext("!=");
            ConditionParser.Tokenise(context);
            var value = Assert.Single(context.Values);
            Assert.IsType<NotEqual>(value);
        }
    }
}
