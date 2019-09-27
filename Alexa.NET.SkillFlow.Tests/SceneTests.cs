using Alexa.NET.SkillFlow.Interpreter;
using Xunit;

namespace Alexa.NET.SkillFlow.Tests
{
    public class SceneTests
    {
        [Fact]
        public void IdentifiesAtSymbolAsCandidate()
        {
            var interpreter = new SceneInterpreter();
            Assert.True(interpreter.CanInterpret("@",new SkillFlowInterpretationContext()));
        }

        [Fact]
        public void FailsToIdentifyCandidate()
        {
            var interpeter = new SceneInterpreter();
            Assert.False(interpeter.CanInterpret("*", new SkillFlowInterpretationContext()));
        }
    }
}
