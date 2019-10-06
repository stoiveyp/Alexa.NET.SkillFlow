using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.IO.Pipelines;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Xml;
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

        public Dictionary<Type, List<ISkillFlowInterpreter>> Interpreters = new Dictionary<Type, List<ISkillFlowInterpreter>>
        {
            {typeof(Story),new List<ISkillFlowInterpreter>(new ISkillFlowInterpreter[]{new SceneInterpreter()}) },
            {typeof(Scene),new List<ISkillFlowInterpreter>(new ISkillFlowInterpreter[]{new ScenePropertyInterpreter()}) },
            {typeof(Text),new List<ISkillFlowInterpreter>(new ISkillFlowInterpreter[]{new MultiLineInterpreter()}) },
            {typeof(Visual),new List<ISkillFlowInterpreter>(new ISkillFlowInterpreter[]{new VisualPropertyInterpreter()}) },
            {typeof(SceneInstructions),new List<ISkillFlowInterpreter>(new ISkillFlowInterpreter[]
            {
                new GoToInterpreter(),
                new HearInterpreter()
            }) },
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


                var currentLevel = 1;
                for (var checkPos = 0; checkPos < osb.Length; checkPos++)
                {
                    if (osb[checkPos] == '\t')
                    {
                        currentLevel++;
                    }
                    else
                    {
                        if (currentLevel > context.Components.Count + 1)
                        {
                            throw new InvalidSkillFlowDefinitionException("Out of place indent", context.LineNumber);
                        }

                        while (currentLevel < context.Components.Count)
                        {
                            context.Components.Pop();
                        }

                        break;
                    }
                }


                currentLevel--;
                var candidate = osb.ToString(currentLevel, osb.Length - currentLevel);

                var used = buffer.Start;

                var interpreter = Interpreters[context.CurrentComponent.GetType()].FirstOrDefault(i => i.CanInterpret(candidate, context));

                if (interpreter != null)
                {
                    try
                    {
                        var result = interpreter.Interpret(candidate, context);
                        if (result.Component == null)
                        {
                            throw new InvalidSkillFlowDefinitionException("Unable to parse", context.LineNumber);
                        }
                        context.CurrentComponent.Add(result.Component);
                        context.Components.Push(result.Component);
                    }
                    catch (InvalidSkillFlowException invalidSkillFlow)
                    {
                        throw new InvalidSkillFlowDefinitionException(invalidSkillFlow.Message, context.LineNumber);
                    }

                    used = buffer.GetPosition(candidate.Length + (hitLineBreak ? context.Options.LineEnding.Length : 0) + currentLevel);
                }
                else
                {
                    throw new InvalidSkillFlowDefinitionException($"Unrecognised skill flow: " + candidate,
                        context.LineNumber);
                }

                reader.AdvanceTo(used, examined);
            }

            return context.Story;
        }
    }
}
