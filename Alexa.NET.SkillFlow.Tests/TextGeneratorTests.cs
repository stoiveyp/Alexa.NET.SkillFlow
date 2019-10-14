using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipelines;
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
            var stream = new MemoryStream();
            var context = new TextGeneratorContext(stream)
            {
                LineEnding = "\n"
            };
            var generator = new TextGenerator.TextGenerator();
            await generator.Generate(story, context);
            var output = ToText(stream);
            Assert.Equal("@test this thing\n",output);
        }

        private string ToText(MemoryStream stream)
        {
            stream.Position = 0;
            return new StreamReader(stream).ReadToEnd();
        }
    }
}
