using System;
using Alexa.NET.SkillFlow.Instructions;
using Alexa.NET.SkillFlow.Interpreter;
using Alexa.NET.SkillFlow.Terminators;
using Xunit;

namespace Alexa.NET.SkillFlow.Tests
{
    public class TerminatorTests
    {
        [Fact]
        public void CorrectlyIdentifiesText()
        {
            var interpreter = new TerminatorInterpreter();
            var context = new SkillFlowInterpretationContext(new SkillFlowInterpretationOptions());
            context.Components.Push(new SceneInstructions());
            Assert.True(interpreter.CanInterpret(">> RESTART", context));
        }

        [Fact]
        public void CorrectlyIdentifiesFalseText()
        {
            var interpreter = new TerminatorInterpreter();
            Assert.False(interpreter.CanInterpret(">>> RESTART", new SkillFlowInterpretationContext(new SkillFlowInterpretationOptions())));
        }

        [Theory]
        [InlineData(">> RESTART", typeof(Restart))]
        [InlineData(">> PAUSE", typeof(Pause))]
        [InlineData(">> RESUME", typeof(Resume))]
        [InlineData(">> REPEAT", typeof(Repeat))]
        [InlineData(">> BACK", typeof(Back))]
        [InlineData(">> END", typeof(End))]
        [InlineData(">> RETURN", typeof(Return))]
        public void CreateTerminatorsComponentCorrectly(string text, Type type)
        {
            var interpreter = new TerminatorInterpreter();
            var result = interpreter.Interpret(text,
                new SkillFlowInterpretationContext(new SkillFlowInterpretationOptions()));
            Assert.IsType(type, result.Component);
        }

        [Fact]
        public void AddsCorrectly()
        {
            var instruction = new Restart();
            var instructions = new SceneInstructions();
            instructions.Add(instruction);
            var result = Assert.Single(instructions.Instructions);
            Assert.Equal(instruction, result);
        }
    }
}
