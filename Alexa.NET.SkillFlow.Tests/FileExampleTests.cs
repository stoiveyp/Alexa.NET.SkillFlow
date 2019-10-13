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
            using (var stream = File.OpenRead("Examples/story.abc"))
            {
                var model = await interpreter.Interpret(stream);
                Assert.NotNull(model);
            }
        }
    }
}
