using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Alexa.NET.SkillFlow.Generator
{
    public abstract class SkillFlowGenerator<TContext>
    {
        public Dictionary<Type, IComponentGenerator<TContext>> Extensions { get; } = new Dictionary<Type, IComponentGenerator<TContext>>();

        protected Task<TContext> Generate(SkillFlowComponent component, TContext context)
        {
            Type currentType = component.GetType();
            while (currentType != null && !Extensions.ContainsKey(currentType))
            {
                currentType = currentType.BaseType;
            }

            if (currentType == null)
            {
                return Task.FromResult(context);
            }
            return Extensions[currentType].Generate(component, context);
        }

        public async Task Generate(Story story, TContext context)
        {
            await BeginStory(story, context);
            foreach(var scene in story.Scenes)
            {
                await Generate(scene.Value, context);
            }
            await EndStory(story, context);
        }

        private async Task Generate(Scene scene, TContext context)
        {
            await BeginScene(scene, context);
            await EndScene(scene, context);
        }

        protected virtual Task BeginScene(Scene scene, TContext context)
        {
            return Noop(context);
        }

        protected virtual Task BeginStory(Story story, TContext context)
        {
            return Noop(context);
        }

        protected virtual Task EndStory(Story story, TContext context)
        {
            return Noop(context);
        }

        protected virtual Task EndScene(Scene scene, TContext context)
        {
            return Noop(context);
        }

        protected virtual Task Noop(TContext context)
        {
            return Task.FromResult(context);
        }
    }
}
