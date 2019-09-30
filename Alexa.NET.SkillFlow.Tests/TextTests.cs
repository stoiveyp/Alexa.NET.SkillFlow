using System;
using System.Collections.Generic;
using System.Text;
using Alexa.NET.SkillFlow.Interpreter;
using Xunit;

namespace Alexa.NET.SkillFlow.Tests
{
    public class TextTests
    {
        [Fact]
        public void InterpreterValidOnNewLine()
        {
            var interpreter = new MultiLineInterpreter();
            Assert.True(interpreter.CanInterpret(string.Empty, new SkillFlowInterpretationContext(new SkillFlowInterpretationOptions())));
        }

        [Fact]
        public void InterpreterInvalidOnPartialLine()
        {
            var interpreter = new MultiLineInterpreter();
            var context = new SkillFlowInterpretationContext(new SkillFlowInterpretationOptions());
            context.BeginningOfLine = false;
            Assert.False(interpreter.CanInterpret(string.Empty, context));
        }

        [Fact]
        public void TextTypeSetCorrectly()
        {
            var testText = "test text";
            var text = new Text(testText);
            Assert.Equal(testText, text.TextType);
        }
    }
}
