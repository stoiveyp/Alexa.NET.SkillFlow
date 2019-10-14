using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipelines;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alexa.NET.SkillFlow.TextGenerator;
using Xunit;

namespace Alexa.NET.SkillFlow.Tests
{
    public class TextGeneratorTests
    {
        [Fact]
        public async Task SceneGeneratesProperly()
        {
            var story = new Story();
            story.Scenes.Add("test", new Scene { Name = "test this thing" });
            story.Scenes.Add("test2", new Scene { Name = "test another thing" });
            var output = await OutputStory(story);
            Assert.Equal("@test this thing\n@test another thing\n", output);
        }

        [Fact]
        public async Task TextGeneratesProperly()
        {
            var story = new Story();
            story.Scenes.Add("test", new Scene
            {
                Name = "test this thing",
            });
            story.Scenes.First().Value.Add(new Text("say")
            {
                Content = new List<string>(new[] { "test", "thing" })
            });
            var output = await OutputStory(story);
            Assert.Equal("@test this thing\n\t*say\n\t\ttest\n\t\tthing\n", output);
        }

        [Fact]
        public async Task CommentsGeneratesProperly()
        {
            var story = new Story();
            story.Scenes.Add("test", new Scene
            {
                Name = "test this thing",
                Comments = new[] {"this is a comment"}
            });
            var output = await OutputStory(story);
            Assert.Equal("//this is a comment\n@test this thing\n", output);
        }

        private async Task<string> OutputStory(Story story)
        {
            var stream = new MemoryStream();
            var context = new TextGeneratorContext(stream)
            {
                LineEnding = "\n"
            };
            var generator = new TextGenerator.TextGenerator();
            await generator.Generate(story, context);
            return ToText(stream);
        }

        private string ToText(MemoryStream stream)
        {
            stream.Position = 0;
            return new StreamReader(stream).ReadToEnd();
        }
    }
}
