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

            var leftWrap = Assert.IsType<ValueWrapper>(equal.Left);
            var leftLiteral = Assert.IsType<LiteralValue>(leftWrap.Value);
            Assert.Equal("3",leftLiteral.Value);

            var rightWrap = Assert.IsType<ValueWrapper>(equal.Right);
            var rightLiteral = Assert.IsType<LiteralValue>(rightWrap.Value);
            Assert.Equal("5", rightLiteral.Value);
        }
    }
}
