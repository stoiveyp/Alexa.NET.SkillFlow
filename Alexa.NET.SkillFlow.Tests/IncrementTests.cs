using System;
using System.Collections.Generic;
using System.Text;
using Alexa.NET.SkillFlow.Instructions;
using Alexa.NET.SkillFlow.Interpreter;
using Xunit;

namespace Alexa.NET.SkillFlow.Tests
{
    public class IncrementTests
    {
        [Fact]
        public void IncreaseCorrectlyIdentifiesText()
        {
            var interpreter = new IncrementInterpreter();
            var context = new SkillFlowInterpretationContext(new SkillFlowInterpretationOptions());
            context.Components.Push(new SceneInstructions());
            Assert.True(interpreter.CanInterpret("increase test by 3", context));
        }

        [Fact]
        public void IncreaseCorrectlyIdentifiesFalseText()
        {
            var interpreter = new IncrementInterpreter();
            Assert.False(interpreter.CanInterpret("increase test to true", new SkillFlowInterpretationContext(new SkillFlowInterpretationOptions())));
        }

        [Fact]
        public void IncreaseCreatesComponentCorrectly()
        {
            var interpreter = new IncrementInterpreter();
            var result = interpreter.Interpret("increase test by 3",
                new SkillFlowInterpretationContext(new SkillFlowInterpretationOptions()));
            var instruction = Assert.IsType<Increase>(result.Component);
            Assert.Equal("test", instruction.Variable);
            Assert.Equal(3, instruction.Amount);
        }

        [Fact]
        public void IncreaseAddsCorrectly()
        {
            var instruction = new Increase("test", 3);
            var instructions = new SceneInstructions();
            instructions.Add(instruction);
            var result = Assert.Single(instructions.Instructions);
            Assert.Equal(instruction, result);
        }

        [Fact]
        public void DecreaseCorrectlyIdentifiesText()
        {
            var interpreter = new IncrementInterpreter();
            var context = new SkillFlowInterpretationContext(new SkillFlowInterpretationOptions());
            context.Components.Push(new SceneInstructions());
            Assert.True(interpreter.CanInterpret("decrease test to 3", context));
        }

        [Fact]
        public void DecreaseCorrectlyIdentifiesFalseText()
        {
            var interpreter = new IncrementInterpreter();
            Assert.False(interpreter.CanInterpret("decrease test to test", new SkillFlowInterpretationContext(new SkillFlowInterpretationOptions())));
        }

        [Fact]
        public void DecreaseCreatesComponentCorrectly()
        {
            var interpreter = new IncrementInterpreter();
            var result = interpreter.Interpret("decrease test by 3",
                new SkillFlowInterpretationContext(new SkillFlowInterpretationOptions()));
            var instruction = Assert.IsType<Decrease>(result.Component);
            Assert.Equal("test", instruction.Variable);
            Assert.Equal(3, instruction.Amount);
        }

        [Fact]
        public void DecreaseAddsCorrectly()
        {
            var instruction = new Decrease("test", 3);
            var instructions = new SceneInstructions();
            instructions.Add(instruction);
            var result = Assert.Single(instructions.Instructions);
            Assert.Equal(instruction, result);
        }
    }
}
