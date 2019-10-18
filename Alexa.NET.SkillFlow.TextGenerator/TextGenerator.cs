using System.Collections.Generic;
using System.Threading.Tasks;
using Alexa.NET.SkillFlow.Conditions;
using Alexa.NET.SkillFlow.Generator;
using Alexa.NET.SkillFlow.Instructions;
using Alexa.NET.SkillFlow.Terminators;

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
            await context.WriteString(" {", true);
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
                case Clear clear:
                    return context.WriteLine($"clear {clear.Variable}");
                case ClearAll all:
                    return context.WriteLine("clear *");
                case Decrease decrease:
                    return context.WriteLine($"increase {decrease.Variable} by {decrease.Amount}");
                case Flag flag:
                    return context.WriteLine($"flag {flag.Variable}");
                case GoTo goTo:
                    return context.WriteLine($"-> {goTo.SceneName}");
                case GoToAndReturn goToAndReturn:
                    return context.WriteLine($"<-> {goToAndReturn}");
                case Increase increase:
                    return context.WriteLine($"increase {increase.Variable} by {increase.Amount}");
                case Set set:
                    return context.WriteLine($"increase {set.Variable} to {set.Value}");
                case SlotAssignment slot:
                    return context.WriteLine($"slot {slot.SlotName} to '{slot.SlotType}'");
                case Unflag unflag:
                    return context.WriteLine($"unflag {unflag.Variable}");
                case Back back:
                    return context.WriteLine(">> BACK");
                case End end:
                    return context.WriteLine(">> END");
                case Pause pause:
                    return context.WriteLine(">> PAUSE");
                case Repeat repeat:
                    return context.WriteLine(">> REPEAT");
                case Reprompt reprompt:
                    return context.WriteLine(">> REPROMPT");
                case Restart restart:
                    return context.WriteLine(">> RESTART");
                case Resume resume:
                    return context.WriteLine(">> RESUME");
                case Return returnvalue:
                    return context.WriteLine(">> RETURN");
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
            if (!context.IsLast)
            {
                return context.WriteLine(string.Empty);
            }

            return base.End(scene, context);
        }

        protected override Task End(Text text, TextGeneratorContext context)
        {
            context.CurrentLevel--;
            return base.End(text, context);
        }
    }
}
