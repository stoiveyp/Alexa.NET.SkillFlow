using Alexa.NET.SkillFlow.Interpreter;
using Xunit;

namespace Alexa.NET.SkillFlow.Tests
{
    public class SceneTests
    {
        private SkillFlowInterpretationContext DefaultContext =>
            new SkillFlowInterpretationContext(new SkillFlowInterpretationOptions());

        [Fact]
        public void IdentifiesAtSymbolAsCandidate()
        {
            var interpreter = new SceneInterpreter();
            Assert.True(interpreter.CanInterpret("@scene", DefaultContext));
        }

        [Fact]
        public void FailsToIdentifyCandidate()
        {
            var interpreter = new SceneInterpreter();
            Assert.False(interpreter.CanInterpret("*scene", DefaultContext));
        }

        [Fact]
        public void ThrowsOnNoName()
        {
            var interpreter = new SceneInterpreter();
            Assert.Throws<InvalidSkillFlowException>(() => interpreter.Interpret("@scene", DefaultContext));
        }

        [Fact]
        public void ThrowsOnInvalidName()
        {
            var interpreter = new SceneInterpreter();
            Assert.Throws<InvalidSkillFlowException>(() => interpreter.Interpret("@scene &&", DefaultContext));
        }

        [Fact]
        public void ReturnCorrectIndex()
        {
            var interpreter = new SceneInterpreter();
            var newIndex = interpreter.Interpret("@scene test", DefaultContext);
            Assert.Equal(11, newIndex);
        }

        [Fact]
        public void AddsSceneToStory()
        {
            var interpreter = new SceneInterpreter();
            var context = DefaultContext;
            interpreter.Interpret("@scene test", context);
            var scene = Assert.Single(context.Story.Scenes);
            Assert.Equal("test", scene.Key);
            Assert.Equal("test", scene.Value.Name);
        }
    }
}
