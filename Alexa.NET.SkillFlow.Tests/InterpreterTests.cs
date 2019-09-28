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
        public async Task ThrowsOnInvalidSkillFlow()
        {
            var ex = await Assert.ThrowsAsync<InvalidSkillFlowException>(() => new SkillFlowInterpreter().Interpret($"@scene test {Environment.NewLine} ~"));
            Assert.Equal(2, ex.LineNumber);
        }

        [Fact]
        public async Task ThrowWhenInterpreterDoesntMove()
        {
            var interpreter = new SkillFlowInterpreter(new NoMoveInterpreter());
            var ex = await Assert.ThrowsAsync<InvalidSkillFlowException>(() => interpreter.Interpret($"@scene test {Environment.NewLine}~"));
            Assert.Equal(2, ex.LineNumber);
        }
    }

    public class NoMoveInterpreter : ISkillFlowInterpreter
    {
        public bool CanInterpret(string candidate, SkillFlowInterpretationContext context)
        {
            return candidate.StartsWith('~');
        }

        public int Interpret(string candidate, SkillFlowInterpretationContext context)
        {
            return 0;
        }
    }
}
