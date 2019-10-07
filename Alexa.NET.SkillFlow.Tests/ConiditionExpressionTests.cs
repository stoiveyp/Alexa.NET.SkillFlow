﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alexa.NET.SkillFlow.Conditions;
using Alexa.NET.SkillFlow.Interpreter;
using Xunit;

namespace Alexa.NET.SkillFlow.Tests
{
    public class ConditionExpressionTests
    {
        [Fact]
        public void EqualityCheck()
        {
            var context = new ConditionContext("defeated == false");

            ConditionParser.Tokenise(context);
            Assert.Equal(3,context.Values.Count);

            var values = context.Values;
            Assert.IsType<LiteralValue>(values[0]);
            Assert.IsType<Equal>(values[1]);
            Assert.IsType<LiteralValue>(values[2]);
        }

        [Fact]
        public void MultipleChecksWithAnd()
        {
            var context = new ConditionContext("defeated == false && 5 > 3");
            ConditionParser.Tokenise(context);
            Assert.Equal(7, context.Values.Count);

            var values = context.Values;
            Assert.IsType<LiteralValue>(values[0]);
            Assert.IsType<Equal>(values[1]);
            Assert.IsType<LiteralValue>(values[2]);
            Assert.IsType<And>(values[3]);
            Assert.IsType<LiteralValue>(values[4]);
            Assert.IsType<GreaterThan>(values[5]);
            Assert.IsType<LiteralValue>(values[6]);
        }
    }
}
