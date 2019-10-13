using System.Linq;
using System.Threading.Tasks;
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

        [Theory]
        [InlineData("@scene test", "test")]
        [InlineData("@start", "start")]
        [InlineData("@resume", "resume")]
        [InlineData("@pause", "pause")]
        [InlineData("@global prepend", "global prepend")]
        [InlineData("@global append", "global append")]
        public async Task GeneratesScene(string text, string name)
        {
            var interpreter = new SkillFlowInterpreter();
            var result = await interpreter.Interpret(text);
            var scene = Assert.IsType<Scene>(result.Scenes.Single().Value);
            Assert.Equal(name, scene.Name);
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
