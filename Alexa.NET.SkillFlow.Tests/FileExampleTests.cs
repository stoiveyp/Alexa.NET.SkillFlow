using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Alexa.NET.SkillFlow.Tests
{
    public class FileExampleTests
    {
        [Fact]
        public async Task TestStoryParse()
        {
            var interpreter = new SkillFlowInterpreter();
            Story story = null;
            using (var stream = File.OpenRead("Examples/story.abc"))
            {
                story = await interpreter.Interpret(stream);
            }

            Assert.NotNull(story);
            Assert.Equal(22,story.Scenes.Count);
        }
    }
}
