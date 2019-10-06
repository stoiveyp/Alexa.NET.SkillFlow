using System;
using System.IO;
using System.IO.Pipelines;
using System.Threading.Tasks;
using Alexa.NET.SkillFlow.Interpreter;
using Xunit;

namespace Alexa.NET.SkillFlow.Tests
{
    public class InterpreterTests
    {
        [Fact]
        public async Task NullStreamThrows()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => new SkillFlowInterpreter().Interpret((Stream)null));
        }

        [Fact]
        public async Task NullPipeThrows()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => new SkillFlowInterpreter().Interpret((PipeReader)null).AsTask());
        }

        [Fact]
        public async Task NullStringThrows()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => new SkillFlowInterpreter().Interpret((string)null));
        }

        [Fact]
        public async Task EmptyStringReturnsEmptySkillFlow()
        {
            var result = await new SkillFlowInterpreter().Interpret(string.Empty);
            Assert.NotNull(result);
            Assert.Empty(result.Scenes);
        }

        [Fact]
        public async Task HandlesNewLine()
        {
            var story = await new SkillFlowInterpreter().Interpret("@scene test" + Environment.NewLine);
            var scene = Assert.Single(story.Scenes);
            Assert.Equal("test", scene.Key);
            Assert.Equal("test", scene.Value.Name);
        }

        [Fact]
        public async Task AddOccursOnCorrectTab()
        {
            var story = await new SkillFlowInterpreter().Interpret("\t@scene test");
            var scene = Assert.Single(story.Scenes);
            Assert.Equal("test", scene.Key);
            Assert.Equal("test", scene.Value.Name);
        }

        [Fact]
        public async Task ThrowsOnIncorrectTab()
        {
            var ex = await Assert.ThrowsAsync<InvalidSkillFlowDefinitionException>(() => new SkillFlowInterpreter().Interpret("\t\t@scene test"));
            Assert.Equal(1,ex.LineNumber);
            Assert.Equal("Out of place indent",ex.Message);
        }

        [Fact]
        public async Task ThrowsOnInvalidSkillFlow()
        {
            var ex = await Assert.ThrowsAsync<InvalidSkillFlowDefinitionException>(() => new SkillFlowInterpreter().Interpret($"@scene test {Environment.NewLine} ~"));
            Assert.Equal(2, ex.LineNumber);
        }

        [Fact]
        public async Task ThrowWhenInterpreterDoesntMove()
        {
            var interpreter = new SkillFlowInterpreter();
            interpreter.Interpreters[typeof(Scene)].Add(new NoMoveInterpreter());
            var ex = await Assert.ThrowsAsync<InvalidSkillFlowDefinitionException>(() => interpreter.Interpret($"@scene test {Environment.NewLine}~"));
            Assert.Equal(2, ex.LineNumber);
        }

        [Fact]
        public async Task MultilineAddsToCorrectComponent()
        {
            var interpreter = new SkillFlowInterpreter();
            var story = await interpreter.Interpret(string.Join(Environment.NewLine, "@scene test",
                "\t*say","\t\twibble","\t\t||","\t\ttest"));
            var scene = Assert.Single(story.Scenes).Value;
            Assert.Null(scene.Reprompt);
            Assert.Null(scene.Recap);
            Assert.NotNull(scene.Say);
            Assert.Equal(2,scene.Say.Content.Count);
        }
    }
}
