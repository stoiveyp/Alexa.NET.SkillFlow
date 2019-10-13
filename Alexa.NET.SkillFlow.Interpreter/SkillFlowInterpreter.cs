using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.IO.Pipelines;
using System.Linq;
using System.Text;
using System.Threading;
using Alexa.NET.SkillFlow.Interpreter;

namespace Alexa.NET.SkillFlow
{
    public class SkillFlowInterpreter
    {
        private readonly SkillFlowInterpretationOptions _options;

        public SkillFlowInterpreter(SkillFlowInterpretationOptions options = null)
        {
            _options = options ?? new SkillFlowInterpretationOptions();
        }

        public List<ISkillFlowInterpreter> CommonInterpreters = new List<ISkillFlowInterpreter>
        {
            new CommentInterpreter()
        };

        public Dictionary<Type, List<ISkillFlowInterpreter>> TypedInterpreters = new Dictionary<Type, List<ISkillFlowInterpreter>>
        {
            {typeof(Story),new List<ISkillFlowInterpreter>(new ISkillFlowInterpreter[]
            {
                new SceneInterpreter(),
                new SpecialSceneInterpreter()
            }) },
            {typeof(Scene),new List<ISkillFlowInterpreter>(new ISkillFlowInterpreter[]{new ScenePropertyInterpreter()}) },
            {typeof(Text),new List<ISkillFlowInterpreter>(new ISkillFlowInterpreter[]{ new ScenePropertyInterpreter(),new MultiLineInterpreter()}) },
            {typeof(Visual),new List<ISkillFlowInterpreter>(new ISkillFlowInterpreter[]{new VisualPropertyInterpreter()}) },
            {typeof(SceneInstructionContainer),new List<ISkillFlowInterpreter>(new ISkillFlowInterpreter[]
            {
                new GoToInterpreter(),
                new HearInterpreter(),
                new IfInterpreter(),
                new SetInterpreter(),
                new IncrementInterpreter(),
                new FlagInterpreter(),
                new TerminatorInterpreter(),
                new ClearInterpreter(),
                new SlotAssignmentInterpreter(), 
                new CloseInstructionGroupInterpreter()
            }) }
        };

        public Task<Story> Interpret(string input, CancellationToken token = default)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(input));
            return Interpret(ms, token);
        }

        public Task<Story> Interpret(Stream input, CancellationToken token = default)
        {
            return Interpret(PipeReader.Create(input), token).AsTask();
        }

        public async ValueTask<Story> Interpret(PipeReader reader, CancellationToken token = default)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            var context = new SkillFlowInterpretationContext(_options);
            var osb = new StringBuilder();

            while (true)
            {
                osb.Clear();
                var readResult = await reader.ReadAsync(token);
                var buffer = readResult.Buffer;
                if (buffer.IsEmpty && readResult.IsCompleted)
                {
                    break;
                }

                var examined = buffer.End;
                var hitLineBreak = false;
                foreach (var segment in buffer)
                {
                    var segmentString = Encoding.UTF8.GetString(segment.ToArray());

                    if (segmentString.Contains(_options.LineEnding))
                    {
                        var cutoff = segmentString.IndexOf(context.Options.LineEnding);
                        osb.Append(segmentString.Substring(0, cutoff));
                        examined = buffer.GetPosition(osb.Length);
                        hitLineBreak = true;
                        break;
                    }

                    osb.Append(segmentString);
                }

                if (!readResult.IsCompleted && !hitLineBreak)
                {
                    reader.AdvanceTo(buffer.Start, buffer.End);
                    continue;
                }

                context.LineNumber++;

                var candidate = osb.ToString().Trim();
                var used = buffer.Start;

                if (string.IsNullOrWhiteSpace(candidate))
                {
                    used = buffer.GetPosition(osb.Length + (hitLineBreak ? context.Options.LineEnding.Length : 0));
                    reader.AdvanceTo(used, examined);
                    continue;
                }

                Type errorType = context.CurrentComponent.GetType();
                Type currentType = null;
                ISkillFlowInterpreter interpreter = null;
                while (currentType == null && context.Components.Any())
                {
                    currentType = context.CurrentComponent.GetType();
                    while (currentType != null && !TypedInterpreters.ContainsKey(currentType))
                    {
                        currentType = currentType.BaseType;
                    }

                    if (currentType != null)
                    {
                        interpreter = CommonInterpreters.Concat(TypedInterpreters[currentType]).FirstOrDefault(i => i.CanInterpret(candidate, context));
                        if (interpreter == null)
                        {
                            currentType = null;
                        }

                    }

                    if (currentType == null)
                    {
                        if (context.CurrentComponent is SceneInstructionContainer container && container.Group)
                        {
                            throw new InvalidSkillFlowDefinitionException("Unclosed group", context.LineNumber);
                        }

                        if (context.Components.Count > 1)
                        {
                            context.Components.Pop();
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                if (currentType == null)
                {
                    throw new InvalidSkillFlowDefinitionException(
                        $"Unknown definition for {errorType.Name} - \"{candidate}\"", context.LineNumber);
                }

                try
                {
                    var result = interpreter.Interpret(candidate, context);
                    if (!InterpreterResult.IsEmpty(result))
                    {
                        result.Component.Comments = context.Comments.ToArray();
                        context.Comments.Clear();
                        context.CurrentComponent.Add(result.Component);
                        context.Components.Push(result.Component);
                    }
                }
                catch (InvalidSkillFlowException invalidSkillFlow)
                {
                    throw new InvalidSkillFlowDefinitionException(invalidSkillFlow.Message, context.LineNumber);
                }

                
                used = buffer.GetPosition(osb.Length + (hitLineBreak ? context.Options.LineEnding.Length : 0));
                reader.AdvanceTo(used, examined);
            }

            return context.Story;
        }
    }
}
