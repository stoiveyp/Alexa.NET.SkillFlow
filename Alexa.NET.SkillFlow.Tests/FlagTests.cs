using System;
using System.Collections.Generic;
using System.Text;
using Alexa.NET.SkillFlow.Instructions;
using Alexa.NET.SkillFlow.Interpreter;
using Xunit;

namespace Alexa.NET.SkillFlow.Tests
{
    public class FlagTests
    {
        [Fact]
        public void FlagCorrectlyIdentifiesText()
        {
            var interpreter = new FlagInterpreter();
            var context = new SkillFlowInterpretationContext(new SkillFlowInterpretationOptions());
            context.Components.Push(new SceneInstructions());
            Assert.True(interpreter.CanInterpret("flag test", context));
        }

        [Fact]
        public void FlagCorrectlyIdentifiesFalFlagext()
        {
            var interpreter = new FlagInterpreter();
            Assert.False(interpreter.CanInterpret("flags test", new SkillFlowInterpretationContext(new SkillFlowInterpretationOptions())));
        }

        [Fact]
        public void FlagCreatesComponentCorrectly()
        {
            var interpreter = new FlagInterpreter();
            var result = interpreter.Interpret("flag test",
                new SkillFlowInterpretationContext(new SkillFlowInterpretationOptions()));
            var instruction = Assert.IsType<Flag>(result.Component);
            Assert.Equal("test", instruction.Variable);
        }

        [Fact]
        public void FlagAddsCorrectly()
        {
            var instruction = new Flag("test");
            var instructions = new SceneInstructions();
            instructions.Add(instruction);
            var result = Assert.Single(instructions.Instructions);
            Assert.Equal(instruction, result);
        }

        [Fact]
        public void UnflagCorrectlyIdentifiesText()
        {
            var interpreter = new FlagInterpreter();
            var context = new SkillFlowInterpretationContext(new SkillFlowInterpretationOptions());
            context.Components.Push(new SceneInstructions());
            Assert.True(interpreter.CanInterpret("unflag test", context));
        }

        [Fact]
        public void UnflagCorrectlyIdentifiesFalUnflagext()
        {
            var interpreter = new FlagInterpreter();
            Assert.False(interpreter.CanInterpret("unflags test", new SkillFlowInterpretationContext(new SkillFlowInterpretationOptions())));
        }

        [Fact]
        public void UnflagCreatesComponentCorrectly()
        {
            var interpreter = new FlagInterpreter();
            var result = interpreter.Interpret("unflag test",
                new SkillFlowInterpretationContext(new SkillFlowInterpretationOptions()));
            var instruction = Assert.IsType<Unflag>(result.Component);
            Assert.Equal("test", instruction.Variable);
        }

        [Fact]
        public void UnflagAddsCorrectly()
        {
            var instruction = new Unflag("test");
            var instructions = new SceneInstructions();
            instructions.Add(instruction);
            var result = Assert.Single(instructions.Instructions);
            Assert.Equal(instruction, result);
        }
    }
}
