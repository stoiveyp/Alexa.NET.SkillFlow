using System;
using System.Collections.Generic;
using System.Text;
using Alexa.NET.SkillFlow.Instructions;
using Alexa.NET.SkillFlow.Interpreter;
using Xunit;

namespace Alexa.NET.SkillFlow.Tests
{
    public class GoToTests
    {
        [Fact]
        public void CorrectlyIdentifiesText()
        {
            var interpreter = new GoToInterpreter();
            var context = new SkillFlowInterpretationContext(new SkillFlowInterpretationOptions());
            context.Components.Push(new SceneInstructions());
            Assert.True(interpreter.CanInterpret("->", context));
        }

        [Fact]
        public void CorrectlyIdentifiesReturn()
        {
            var interpreter = new GoToInterpreter();
            var context = new SkillFlowInterpretationContext(new SkillFlowInterpretationOptions());
            context.Components.Push(new SceneInstructions());
            Assert.True(interpreter.CanInterpret("<->", context));
        }

        [Fact]
        public void CorrectlyIdentifiesFalseText()
        {
            var interpreter = new GoToInterpreter();
            Assert.False(interpreter.CanInterpret("=>", new SkillFlowInterpretationContext(new SkillFlowInterpretationOptions())));
        }

        [Fact]
        public void CreatesComponentCorrectly()
        {
            var interpreter = new GoToInterpreter();
            var result = interpreter.Interpret("-> test",
                new SkillFlowInterpretationContext(new SkillFlowInterpretationOptions()));
            var instruction = Assert.IsType<GoTo>(result.Component);
            Assert.Equal("test",instruction.SceneName);
        }

        [Fact]
        public void CreatesAndReturnComponentCorrectly()
        {
            var interpreter = new GoToInterpreter();
            var result = interpreter.Interpret("<-> test this thing",
                new SkillFlowInterpretationContext(new SkillFlowInterpretationOptions()));
            var gtar = Assert.IsType<GoToAndReturn>(result.Component);
            Assert.Equal("test this thing", gtar.SceneName);
        }

        [Fact]
        public void AddsCorrectly()
        {
            var instruction = new GoTo("test");
            var instructions = new SceneInstructions();
            instructions.Add(instruction);
            var result = Assert.Single(instructions.Instructions);
            Assert.Equal(instruction,result);
        }

        [Fact]
        public void AddsAndReturnCorrectly()
        {
            var instruction = new GoToAndReturn();
            var instructions = new SceneInstructions();
            instructions.Add(instruction);
            var result = Assert.Single(instructions.Instructions);
            Assert.Equal(instruction, result);
        }
    }
}
