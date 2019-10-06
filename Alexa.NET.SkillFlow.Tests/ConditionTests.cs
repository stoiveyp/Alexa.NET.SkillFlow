using System;
using System.Collections.Generic;
using System.Text;
using Alexa.NET.SkillFlow.Conditions;
using Alexa.NET.SkillFlow.Interpreter;
using Xunit;

namespace Alexa.NET.SkillFlow.Tests
{
    public class ConditionTests
    {
        [Fact]
        public void InvalidConditionThrows()
        {
            Assert.Throws<InvalidConditionException>(() => ConditionParser.Parse("~"));
        }

        [Fact]
        public void EmptyConditionEqualsFalse()
        {
            var condition = ConditionParser.Parse(string.Empty);
            var wrapper = Assert.IsType<ValueWrapper>(condition);
            Assert.IsType<False>(wrapper.Value);
        }
    }
}
