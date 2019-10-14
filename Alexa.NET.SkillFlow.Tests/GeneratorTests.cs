using System.Threading.Tasks;
using NSubstitute;
using Xunit;

namespace Alexa.NET.SkillFlow.Tests
{
    public class GeneratorTests
    {
        [Fact]
        public async Task GeneratorCallsOncePerScene()
        {
            var story = new Story();
            story.Scenes.Add("test", new Scene { Name = "test" });
            story.Scenes.Add("test2", new Scene { Name = "test2" });

            var assertion = new AssertionCallCount();

            var generator = new AssertionGenerator();
            await generator.Generate(story, assertion);

            Assert.Equal(2,assertion.SceneBegin);
            Assert.Equal(2, assertion.SceneEnd);
            Assert.Equal(1, assertion.StoryBegin);
            Assert.Equal(1, assertion.StoryEnd);
        }
    }
}
