using System;
using System.Collections.Generic;
using System.Text;
using Alexa.NET.SkillFlow.Interpreter;
using Xunit;

namespace Alexa.NET.SkillFlow.Tests
{
    public class VisualTests
    {
        [Fact]
        public void ThrowsOnNonVisualProperty()
        {
            var visual = new Visual();
            Assert.Throws<InvalidSkillFlowException>(() => visual.Add(new Scene("test")));
        }

        [Fact]
        public void TemplateSetCorrectly()
        {
            var visual = new Visual();
            visual.Add(new VisualProperty("template","test"));
            Assert.Equal("test",visual.Template);
        }

        [Fact]
        public void BackgroundSetCorrectly()
        {
            var visual = new Visual();
            visual.Add(new VisualProperty("background","test"));
            Assert.Equal("test",visual.Background);
        }

        [Fact]
        public void TitleSetCorrectly()
        {
            var visual = new Visual();
            visual.Add(new VisualProperty("title", "test"));
            Assert.Equal("test", visual.Title);
        }

        [Fact]
        public void SubtitleSetCorrectly()
        {
            var visual = new Visual();
            visual.Add(new VisualProperty("subtitle", "test"));
            Assert.Equal("test", visual.Subtitle);
        }

        [Fact]
        public void InterpreterCandidatesCorrectly()
        {
            var interpreter = new VisualPropertyInterpreter();
            Assert.True(interpreter.CanInterpret("template:''",new SkillFlowInterpretationContext(new SkillFlowInterpretationOptions())));
        }
    }
}
