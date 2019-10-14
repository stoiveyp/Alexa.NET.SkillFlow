using System.Threading.Tasks;
using Alexa.NET.SkillFlow.Generator;

namespace Alexa.NET.SkillFlow.Tests
{
    public class AssertionGenerator : SkillFlowGenerator<AssertionCallCount>
    {
        protected override Task BeginScene(Scene scene, AssertionCallCount context)
        {
            context.SceneBegin++;
            return base.BeginScene(scene, context);
        }

        protected override Task BeginStory(Story story, AssertionCallCount context)
        {
            context.StoryBegin++;
            return base.BeginStory(story, context);
        }

        protected override Task EndScene(Scene scene, AssertionCallCount context)
        {
            context.SceneEnd++;
            return base.EndScene(scene, context);
        }

        protected override Task EndStory(Story story, AssertionCallCount context)
        {
            context.StoryEnd++;
            return base.EndStory(story, context);
        }
    }
}