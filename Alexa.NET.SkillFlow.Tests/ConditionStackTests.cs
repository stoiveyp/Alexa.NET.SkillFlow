using System;
using System.Collections.Generic;
using System.Text;
using Alexa.NET.SkillFlow.Conditions;
using Alexa.NET.SkillFlow.Interpreter;
using Xunit;

namespace Alexa.NET.SkillFlow.Tests
{
    public class ConditionStackTests
    {
        [Fact]
        public void LiteralProducesWrapper()
        {
            var result = ConditionParser.Stack(new Stack<Value>(new[] {new LiteralValue("5")}),"5");
        }

        [Fact]
        public void EqualProducesCorrectBinary()
        {
            var result = ConditionParser.Parse("3 == 5");
            var equal = Assert.IsType<Equal>(result);

            var left = Assert.IsType<LiteralValue>(equal.Left);
            Assert.Equal("3",left.Value);

            var right = Assert.IsType<LiteralValue>(equal.Right);
            Assert.Equal("5", right.Value);
        }

        [Fact]
        public void GroupWithAdd()
        {
            var result = ConditionParser.Parse("(false == test) == (5 > 3)");
            var equal = Assert.IsType<Equal>(result);
            Assert.IsType<Equal>(equal.Left);
            Assert.IsType<GreaterThan>(equal.Right);
        }
    }
}
