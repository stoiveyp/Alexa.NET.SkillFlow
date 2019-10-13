using System;
using System.Collections.Generic;
using System.Text;
using Alexa.NET.SkillFlow.Instructions;
using Alexa.NET.SkillFlow.Interpreter;
using Xunit;

namespace Alexa.NET.SkillFlow.Tests
{
    public class SlotAssignmentTests
    {
        [Fact]
        public void CorrectlyIdentifiesText()
        {
            var interpreter = new SlotAssignmentInterpreter();
            var context = new SkillFlowInterpretationContext(new SkillFlowInterpretationOptions());
            context.Components.Push(new SceneInstructions());
            Assert.True(interpreter.CanInterpret("slot bottles to 'AMAZON.Number'", context));
        }

        [Fact]
        public void CorrectlyIdentifiesFalseText()
        {
            var interpreter = new SlotAssignmentInterpreter();
            Assert.False(interpreter.CanInterpret("slot bottles to AMAZON.Number", new SkillFlowInterpretationContext(new SkillFlowInterpretationOptions())));
        }

        [Fact]
        public void CreatesComponentCorrectly()
        {
            var interpreter = new SlotAssignmentInterpreter();
            var result = interpreter.Interpret("slot bottles to 'AMAZON.Number'",
                new SkillFlowInterpretationContext(new SkillFlowInterpretationOptions()));
            var instruction = Assert.IsType<SlotAssignment>(result.Component);
            Assert.Equal("bottles", instruction.SlotName);
            Assert.Equal("AMAZON.Number", instruction.SlotType);
        }

        [Fact]
        public void AddsCorrectly()
        {
            var instruction = new SlotAssignment("test", "AMAZON.Number");
            var instructions = new SceneInstructions();
            instructions.Add(instruction);
            var result = Assert.Single(instructions.Instructions);
            Assert.Equal(instruction, result);
        }
    }
}
