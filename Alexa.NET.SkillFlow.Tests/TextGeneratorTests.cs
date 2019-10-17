using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Alexa.NET.SkillFlow.Instructions;
using Alexa.NET.SkillFlow.Interpreter;
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
                Comments = new[] { "this is a comment" }
            });
            var output = await OutputStory(story);
            Assert.Equal("//this is a comment\n@test this thing\n", output);
        }

        [Fact]
        public async Task VisualGeneratesProperly()
        {
            var story = new Story();
            var scene = new Scene { Name = "test this thing" };
            var visual = new Visual();
            visual.Add(new VisualProperty("template", "default"));
            scene.Add(visual);
            story.Scenes.Add("test", scene);
            var output = await OutputStory(story);
            Assert.Equal("@test this thing\n\t*show\n\t\ttemplate: 'default'\n", output);
        }

        [Fact]
        public Task IncreaseGeneratesProperly()
        {
            return TestInstruction(new Increase("test", 5), "increase test by 5");
        }

        [Fact]
        public Task HearGeneratesProperly()
        {
            return TestInstruction(new Hear("go north", "go west"), "hear go north, go west {\n\t\t}");
        }

        [Fact]
        public Task IfGeneratesProperly()
        {
            var condition = ConditionParser.Parse("(false == test) == (5 > 3)");
            return TestInstruction(new If(condition), "if ( false == test ) == ( 5 > 3 ) {\n\t\t}");
        }

        public async Task TestInstruction(SceneInstruction instruction, string expectedOutput)
        {
            var story = new Story();
            var scene = new Scene { Name = "test this thing" };
            scene.Add(new SceneInstructions());
            scene.Instructions.Add(instruction);
            story.Scenes.Add("test", scene);
            var output = await OutputStory(story);
            Assert.Equal($"@test this thing\n\t*then\n\t\t{expectedOutput}\n", output);
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
