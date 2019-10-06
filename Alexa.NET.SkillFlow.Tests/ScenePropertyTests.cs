using System;
using System.Collections.Generic;
using System.Text;
using Alexa.NET.SkillFlow.Interpreter;
using Xunit;

namespace Alexa.NET.SkillFlow.Tests
{
    public class ScenePropertyTests
    {
        [Fact]
        public void CorrectlyIdentifiesText()
        {
            var interpreter = new ScenePropertyInterpreter();
            Assert.True(interpreter.CanInterpret("*say",new SkillFlowInterpretationContext(new SkillFlowInterpretationOptions())));
        }

        [Fact]
        public void CorrectlyIdentifiesFalseText()
        {
            var interpreter = new ScenePropertyInterpreter();
            Assert.False(interpreter.CanInterpret("*nosay", new SkillFlowInterpretationContext(new SkillFlowInterpretationOptions())));
        }

        [Fact]
        public void SetSayCorrectly()
        {
            var scene = new Scene("test");
            var say = new Text("say");
            scene.Add(say);
            Assert.Equal(say,scene.Say);
        }

        [Fact]
        public void SetRepromptCorrectly()
        {
            var scene = new Scene("test");
            var reprompt = new Text("reprompt");
            scene.Add(reprompt);
            Assert.Equal(reprompt, scene.Reprompt);
        }

        [Fact]
        public void SetRecapCorrectly()
        {
            var scene = new Scene("test");
            var recap = new Text("recap");
            scene.Add(recap);
            Assert.Equal(recap, scene.Recap);
        }

        [Fact]
        public void SetVisualCorrectly()
        {
            var scene = new Scene("test");
            var show = new Visual();
            scene.Add(show);
            Assert.Equal(show,scene.Visual);
        }

        [Fact]
        public void SetInstructionsCorrectly()
        {
            var scene = new Scene("test");
            var instruction = new SceneInstructions();
            scene.Add(instruction);
            Assert.Equal(instruction, scene.Instructions);
        }

        [Fact]
        public void IdentifiesShow()
        {
            var interpreter = new ScenePropertyInterpreter();
            Assert.True(interpreter.CanInterpret("*say", new SkillFlowInterpretationContext(new SkillFlowInterpretationOptions())));
        }

        [Fact]
        public void ShowReturnsVisual()
        {
            var interpreter = new ScenePropertyInterpreter();
            var result = interpreter.Interpret("*show", new SkillFlowInterpretationContext(new SkillFlowInterpretationOptions()));
            Assert.IsType<Visual>(result.Component);
        }
    }
}
