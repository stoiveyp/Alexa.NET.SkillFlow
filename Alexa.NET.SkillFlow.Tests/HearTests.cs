using System;
using System.Collections.Generic;
using System.Text;
using Alexa.NET.SkillFlow.Instructions;
using Alexa.NET.SkillFlow.Interpreter;
using Xunit;

namespace Alexa.NET.SkillFlow.Tests
{
    public class HearTests
    {
        [Fact]
        public void CorrectlyIdentifiesText()
        {
            var interpreter = new HearInterpreter();
            var context = new SkillFlowInterpretationContext(new SkillFlowInterpretationOptions());
            context.Components.Push(new SceneInstructions());
            Assert.True(interpreter.CanInterpret("hear test {", context));
        }

        [Fact]
        public void CorrectlyIdentifiesFalseText()
        {
            var interpreter = new HearInterpreter();
            Assert.False(interpreter.CanInterpret("hearing", new SkillFlowInterpretationContext(new SkillFlowInterpretationOptions())));
        }

        [Fact]
        public void CreatesComponentWithSinglePhraseCorrectly()
        {
            var interpreter = new HearInterpreter();
            var result = interpreter.Interpret("hear test {",
                new SkillFlowInterpretationContext(new SkillFlowInterpretationOptions()));
            var hear = Assert.IsType<Hear>(result.Component);
            var phrase = Assert.Single(hear.Phrases);
            Assert.Equal("test", phrase);
        }

        [Fact]
        public void CreatesComponentWithMultiplePhraseCorrectly()
        {
            var interpreter = new HearInterpreter();
            var result = interpreter.Interpret("hear test,do the test,testing {",
                new SkillFlowInterpretationContext(new SkillFlowInterpretationOptions()));
            var hear = Assert.IsType<Hear>(result.Component);
            Assert.Equal(3, hear.Phrases.Count);
        }

        [Fact]
        public void AddsCorrectly()
        {
            var hear = new Hear("test");
            var instructions = new SceneInstructions();
            instructions.Add(hear);
            var result = Assert.Single(instructions.Instructions);
            Assert.Equal(hear, result);
        }
    }
}
