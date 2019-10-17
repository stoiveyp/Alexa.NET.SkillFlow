using System.Collections.Generic;
using System.Threading.Tasks;
using Alexa.NET.SkillFlow.Conditions;
using Alexa.NET.SkillFlow.Generator;
using Alexa.NET.SkillFlow.Instructions;

namespace Alexa.NET.SkillFlow.TextGenerator
{
    public class TextGenerator : SkillFlowGenerator<TextGeneratorContext>
    {
        protected override async Task Begin(Scene scene, TextGeneratorContext context)
        {
            await context.WriteLine($"@{scene.Name}");
            context.CurrentLevel++;
        }

        protected override async Task RenderComment(SkillFlowComponent component, TextGeneratorContext context)
        {
            foreach (var comment in component.Comments)
            {
                await context.WriteLine($"//{comment}");
            }
        }

        protected override async Task Begin(Visual story, TextGeneratorContext context)
        {
            await context.WriteLine("*show");
            context.CurrentLevel++;
        }

        protected override Task End(Visual story, TextGeneratorContext context)
        {
            context.CurrentLevel--;
            return Noop(context);
        }

        protected override async Task Begin(SceneInstructions instructions, TextGeneratorContext context)
        {
            await context.WriteLine("*then");
            context.CurrentLevel++;
        }

        protected override async Task Begin(SceneInstructionContainer instructions, TextGeneratorContext context)
        {
            await context.WriteIndent();
            switch (instructions)
            {
                case If ifInstruction:
                    await context.WriteString("if ");
                    await RenderCondition(ifInstruction.Condition, context);
                    break;
                case Hear hear:
                    await context.WriteString("hear ");
                    var existingPhrase = false;
                    foreach (var phrase in hear.Phrases)
                    {
                        if (existingPhrase)
                        {
                            await context.WriteString(", ");
                        }
                        await context.WriteString(phrase);
                        existingPhrase = true;
                    }
                    break;
            }
            await context.WriteString(" {",true);
            context.CurrentLevel++;
        }

        private Task RenderCondition(Condition condition, TextGeneratorContext context)
        {
            return TextCondition.Render(condition, context);
        }

        protected override async Task End(SceneInstructionContainer instructions, TextGeneratorContext context)
        {
            context.CurrentLevel--;
            await context.WriteLine("}");
        }

        protected override Task End(SceneInstructions instructions, TextGeneratorContext context)
        {
            context.CurrentLevel--;
            return Noop(context);
        }

        protected override Task Render(SceneInstruction instruction, TextGeneratorContext context)
        {
            switch (instruction)
            {
                case Increase increase:
                    return context.WriteLine($"increase {increase.Variable} by {increase.Amount}");
                default:
                    return Noop(context);

            }
        }

        protected override async Task Render(VisualProperty story, TextGeneratorContext context)
        {
            await context.WriteLine($"{story.Key}: \'{story.Value}\'");
        }

        protected override async Task Begin(Text text, TextGeneratorContext context)
        {
            await context.WriteLine($"*{text.TextType}");
            context.CurrentLevel++;
        }

        protected override async Task RenderText(List<string> textContent, TextGeneratorContext context)
        {
            foreach (var speech in textContent)
            {
                await context.WriteLine(speech);
            }
        }

        protected override Task End(Scene scene, TextGeneratorContext context)
        {
            context.CurrentLevel--;
            return Noop(context);
        }

        protected override Task End(Text text, TextGeneratorContext context)
        {
            context.CurrentLevel--;
            return base.End(text, context);
        }
    }
}
