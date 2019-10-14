using System;
using System.Collections.Generic;
using System.Linq;
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
            await Begin(story, context);
            foreach (var scene in story.Scenes)
            {
                await Generate(scene.Value, context);
            }
            await End(story, context);
        }

        protected async Task Generate(Text text, TContext context)
        {
            if (text == null)
            {
                return;
            }

            await Begin(text, context);
            await RenderText(text.Content, context);
            await End(text, context);
        }

        protected async Task Generate(Visual visual, TContext context)
        {
            if (visual == null)
            {
                return;
            }

            await Begin(visual, context);
            if (visual.Template != null)
            {
                await Begin(visual.Template, context);
                await End(visual.Template, context);
            }

            if (visual.Background != null)
            {
                await Begin(visual.Background, context);
                await End(visual.Background, context);
            }

            if (visual.Title != null)
            {
                await Begin(visual.Title, context);
                await End(visual.Title, context);
            }

            if (visual.Subtitle != null)
            {
                await Begin(visual.Subtitle, context);
                await End(visual.Subtitle, context);
            }

            await End(visual, context);
        }

        protected virtual Task RenderText(List<string> textContent, TContext context)
        {
            return Noop(context);
        }

        protected virtual Task RenderComment(SkillFlowComponent component, TContext context)
        {
            return Noop(context);
        }

        private async Task Generate(Scene scene, TContext context)
        {
            await GenerateComment(scene, context);
            await Begin(scene, context);

            await GenerateComment(scene.Say, context);
            await Generate(scene.Say, context);

            await GenerateComment(scene.Recap, context);
            await Generate(scene.Recap, context);

            await GenerateComment(scene.Reprompt, context);
            await Generate(scene.Reprompt, context);

            await GenerateComment(scene.Visual, context);
            await Generate(scene.Visual, context);

            await End(scene, context);
        }

        private Task GenerateComment(SkillFlowComponent component, TContext context)
        {
            if (component?.Comments == null || !component.Comments.Any())
            {
                return Noop(context);
            }

            return RenderComment(component, context);
        }

        protected virtual Task Begin(Text text, TContext context)
        {
            return Noop(context);
        }

        protected virtual Task End(Text text, TContext context)
        {
            return Noop(context);
        }

        protected virtual Task Begin(Scene scene, TContext context)
        {
            return Noop(context);
        }

        protected virtual Task Begin(Story story, TContext context)
        {
            return Noop(context);
        }

        protected virtual Task End(Story story, TContext context)
        {
            return Noop(context);
        }

        protected virtual Task Begin(Visual story, TContext context)
        {
            return Noop(context);
        }

        protected virtual Task End(Visual story, TContext context)
        {
            return Noop(context);
        }

        protected virtual Task Begin(VisualProperty story, TContext context)
        {
            return Noop(context);
        }

        protected virtual Task End(VisualProperty story, TContext context)
        {
            return Noop(context);
        }

        protected virtual Task End(Scene scene, TContext context)
        {
            return Noop(context);
        }

        protected virtual Task Noop(TContext context)
        {
            return Task.FromResult(context);
        }
    }
}
