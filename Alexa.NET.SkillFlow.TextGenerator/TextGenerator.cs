using System.Collections.Generic;
using System.Threading.Tasks;
using Alexa.NET.SkillFlow.Generator;

namespace Alexa.NET.SkillFlow.TextGenerator
{
    public class TextGenerator:SkillFlowGenerator<TextGeneratorContext>
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
