using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Alexa.NET.SkillFlow.Generator
{
    public abstract class SkillFlowGenerator<TContext> where TContext : SkillFlowContext
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
            var index = 0;
            foreach (var scene in story.Scenes)
            {
                context.SetLoop(index, story.Scenes.Count - 1);
                await GenerateComment(scene.Value, context);
                await Generate(scene.Value, context);
                context.ClearLoop();
                index++;
            }

            await End(story, context);
        }
        public async Task Generate(SceneInstructions instructions, TContext context)
        {
            if (instructions == null)
            {
                return;
            }
            await Begin(instructions, context);
            var instructionIndex = 0;
            foreach (var instruction in instructions.Instructions)
            {
                context.SetLoop(instructionIndex, instructions.Instructions.Count - 1);
                if (instruction is SceneInstructionContainer container)
                {
                    await GenerateComment(instruction, context);
                    await Generate(container, context);
                    context.ClearLoop();
                    continue;
                }
                await GenerateComment(instruction, context);
                await Render(instruction, context);
                context.ClearLoop();
            }
            await End(instructions, context);
        }
        public async Task Generate(SceneInstructionContainer instructions, TContext context)
        {
            if (instructions == null)
            {
                return;
            }
            await Begin(instructions, context);
            var instructionIndex = 0;
            foreach (var instruction in instructions.Instructions)
            {
                context.SetLoop(instructionIndex, instructions.Instructions.Count - 1);
                if (instruction is SceneInstructionContainer container)
                {
                    await GenerateComment(instruction, context);
                    await Generate(container, context);
                    context.ClearLoop();
                    continue;
                }
                await GenerateComment(instruction, context);
                await Render(instruction, context);
                context.ClearLoop();
            }
            await End(instructions, context);
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
                await GenerateComment(visual.Template, context);
                await Render(visual.Template, context);
            }

            if (visual.Background != null)
            {
                await GenerateComment(visual.Background, context);
                await Render(visual.Background, context);
            }

            if (visual.Title != null)
            {
                await GenerateComment(visual.Title, context);
                await Render(visual.Title, context);
            }

            if (visual.Subtitle != null)
            {
                await GenerateComment(visual.Subtitle, context);
                await Render(visual.Subtitle, context);
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
            await Begin(scene, context);

            await GenerateComment(scene.Say, context);
            await Generate(scene.Say, context);

            await GenerateComment(scene.Recap, context);
            await Generate(scene.Recap, context);

            await GenerateComment(scene.Reprompt, context);
            await Generate(scene.Reprompt, context);

            await GenerateComment(scene.Visual, context);
            await Generate(scene.Visual, context);

            await GenerateComment(scene.Instructions, context);
            await Generate(scene.Instructions, context);

            await End(scene, context);
        }

        protected Task GenerateComment(SkillFlowComponent component, TContext context)
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

        protected virtual Task Render(VisualProperty story, TContext context)
        {
            return Noop(context);
        }

        protected virtual Task Begin(SceneInstructions instructions, TContext context)
        {
            return Noop(context);
        }

        protected virtual Task End(SceneInstructions instructions, TContext context)
        {
            return Noop(context);
        }

        protected virtual Task Begin(SceneInstructionContainer instructions, TContext context)
        {
            return Noop(context);
        }

        protected virtual Task End(SceneInstructionContainer instructions, TContext context)
        {
            return Noop(context);
        }

        protected virtual Task Render(SceneInstruction instruction, TContext context)
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
