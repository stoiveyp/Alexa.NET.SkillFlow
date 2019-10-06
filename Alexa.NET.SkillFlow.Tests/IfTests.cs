using System;
using System.Collections.Generic;
using System.Text;
using Alexa.NET.SkillFlow.Conditions;
using Alexa.NET.SkillFlow.Instructions;
using Alexa.NET.SkillFlow.Interpreter;
using Xunit;

namespace Alexa.NET.SkillFlow.Tests
{
    public class IfTests
    {
        [Fact]
        public void CorrectlyIdentifiesText()
        {
            var interpreter = new IfInterpreter();
            var context = new SkillFlowInterpretationContext(new SkillFlowInterpretationOptions());
            context.Components.Push(new SceneInstructions());
            Assert.True(interpreter.CanInterpret("if test {", context));
        }

        [Fact]
        public void CorrectlyIdentifiesFalseText()
        {
            var interpreter = new IfInterpreter();
            Assert.False(interpreter.CanInterpret("iffy", new SkillFlowInterpretationContext(new SkillFlowInterpretationOptions())));
        }

        [Fact]
        public void AddsCorrectly()
        {
            var hear = new If(new True());
            var instructions = new SceneInstructions();
            instructions.Add(hear);
            var result = Assert.Single(instructions.Instructions);
            Assert.Equal(hear, result);
        }
    }
}
