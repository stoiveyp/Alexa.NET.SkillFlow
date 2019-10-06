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
            Assert.Throws<InvalidSkillFlowDefinitionException>(() => interpreter.Interpret("@scene", DefaultContext));
        }

        [Fact]
        public void ThrowsOnInvalidName()
        {
            var interpreter = new SceneInterpreter();
            Assert.Throws<InvalidSkillFlowDefinitionException>(() => interpreter.Interpret("@scene &&", DefaultContext));
        }

        [Fact]
        public void GeneratesScene()
        {
            var interpreter = new SceneInterpreter();
            var context = DefaultContext;
            var result = interpreter.Interpret("@scene test", context);
            var scene = Assert.IsType<Scene>(result.Component);
            Assert.Equal("test", scene.Name);
        }

        [Fact]
        public void StoryAddsScene()
        {
            var sceneName = "test";
            var story = new Story();
            var scene = new Scene(sceneName);
            story.Add(scene);
            var single = Assert.Single(story.Scenes);
            Assert.Equal(sceneName,single.Key);
            Assert.Equal(scene,single.Value);
        }
    }
}
