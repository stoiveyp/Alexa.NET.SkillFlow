using System;
using System.IO;
using System.IO.Pipelines;
using System.Net.Mail;
using System.Threading.Tasks;
using Alexa.NET.SkillFlow.Generator;

namespace Alexa.NET.SkillFlow.TextGenerator
{
    public class TextGenerator:SkillFlowGenerator<TextGeneratorContext>
    {
        protected override async Task BeginScene(Scene scene, TextGeneratorContext context)
        {
            await context.WriteLine($"@{scene.Name}");
            context.CurrentLevel += 1;
        }

        protected override Task EndScene(Scene scene, TextGeneratorContext context)
        {
            context.CurrentLevel -= 1;
            return Noop(context);
        }
    }
}
