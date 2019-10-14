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
            visual.Add(new VisualProperty("template", "test"));
            Assert.Equal("test", visual.Template.Value);
        }

        [Fact]
        public void BackgroundSetCorrectly()
        {
            var visual = new Visual();
            visual.Add(new VisualProperty("background", "test"));
            Assert.Equal("test", visual.Background.Value);
        }

        [Fact]
        public void TitleSetCorrectly()
        {
            var visual = new Visual();
            visual.Add(new VisualProperty("title", "test"));
            Assert.Equal("test", visual.Title.Value);
        }

        [Fact]
        public void SubtitleSetCorrectly()
        {
            var visual = new Visual();
            visual.Add(new VisualProperty("subtitle", "test"));
            Assert.Equal("test", visual.Subtitle.Value);
        }

        [Fact]
        public void PropertyInterpreterIdentifiesCandidatesCorrectly()
        {
            var interpreter = new VisualPropertyInterpreter();
            var context = new SkillFlowInterpretationContext(new SkillFlowInterpretationOptions());
            context.Components.Push(new Visual());
            Assert.True(interpreter.CanInterpret("template:''", context));
        }

        [Fact]
        public void InterpreterCreatesProperly()
        {
            var interpreter = new VisualPropertyInterpreter();
            var context = new SkillFlowInterpretationContext(new SkillFlowInterpretationOptions());
            context.Components.Push(new Visual());
            var result = interpreter.Interpret("template:'test'", context);
            Assert.NotNull(result.Component);
            var component = Assert.IsType<VisualProperty>(result.Component);
            Assert.Equal("template", component.Key);
            Assert.Equal("test",component.Value);
        }
    }
}
