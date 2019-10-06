using System;
using System.Collections.Generic;
using System.Text;
using Alexa.NET.SkillFlow.Interpreter;
using Xunit;

namespace Alexa.NET.SkillFlow.Tests
{
    public class SceneInstructionTests
    {
        [Fact]
        public void CorrectlyIdentifiesText()
        {
            var interpreter = new ScenePropertyInterpreter();
            Assert.True(interpreter.CanInterpret("*then", new SkillFlowInterpretationContext(new SkillFlowInterpretationOptions())));
        }

        [Fact]
        public void CorrectlyIdentifiesFalseText()
        {
            var interpreter = new ScenePropertyInterpreter();
            Assert.False(interpreter.CanInterpret("*nothen", new SkillFlowInterpretationContext(new SkillFlowInterpretationOptions())));
        }

        [Fact]
        public void CreatesComponentCorrectly()
        {
            var interpreter = new ScenePropertyInterpreter();
            var result = interpreter.Interpret("*then",
                new SkillFlowInterpretationContext(new SkillFlowInterpretationOptions()));
            var list = Assert.IsType<SceneInstructions>(result.Component);
            Assert.Empty(list.Instructions);
        }
    }
}
