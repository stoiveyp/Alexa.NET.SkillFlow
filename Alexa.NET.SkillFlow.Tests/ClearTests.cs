using System;
using System.Collections.Generic;
using System.Text;
using Alexa.NET.SkillFlow.Instructions;
using Alexa.NET.SkillFlow.Interpreter;
using Xunit;

namespace Alexa.NET.SkillFlow.Tests
{
    public class ClearTests
    {
        [Fact]
        public void CorrectlyIdentifiesText()
        {
            var interpreter = new ClearInterpreter();
            var context = new SkillFlowInterpretationContext(new SkillFlowInterpretationOptions());
            context.Components.Push(new SceneInstructions());
            Assert.True(interpreter.CanInterpret("clear test", context));
        }

        [Fact]
        public void CorrectlyIdentifiesFalClearext()
        {
            var interpreter = new ClearInterpreter();
            Assert.False(interpreter.CanInterpret("cleary test", new SkillFlowInterpretationContext(new SkillFlowInterpretationOptions())));
        }

        [Fact]
        public void CreatesComponentCorrectly()
        {
            var interpreter = new ClearInterpreter();
            var result = interpreter.Interpret("clear test",
                new SkillFlowInterpretationContext(new SkillFlowInterpretationOptions()));
            var instruction = Assert.IsType<Clear>(result.Component);
            Assert.Equal("test", instruction.Variable);
        }

        [Fact]
        public void CreatesAllComponentCorrectly()
        {
            var interpreter = new ClearInterpreter();
            var result = interpreter.Interpret("clear *",
                new SkillFlowInterpretationContext(new SkillFlowInterpretationOptions()));
            Assert.IsType<ClearAll>(result.Component);
        }

        [Fact]
        public void AddsCorrectly()
        {
            var instruction = new Clear("test");
            var instructions = new SceneInstructions();
            instructions.Add(instruction);
            var result = Assert.Single(instructions.Instructions);
            Assert.Equal(instruction, result);
        }
    }
}
