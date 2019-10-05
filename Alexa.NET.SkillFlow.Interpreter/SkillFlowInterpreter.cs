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
        private SkillFlowInterpretationOptions _options;

        public SkillFlowInterpreter(SkillFlowInterpretationOptions options,
            params ISkillFlowInterpreter[] customInterpreters)
        {
            Interpreters.AddRange(customInterpreters);
            _options = options ?? new SkillFlowInterpretationOptions();
        }

        public SkillFlowInterpreter(params ISkillFlowInterpreter[] customInterpreters) : this(new SkillFlowInterpretationOptions(), customInterpreters)
        {

        }

        public List<ISkillFlowInterpreter> Interpreters = new List<ISkillFlowInterpreter>()
        {
            new SceneInterpreter(),
            new ScenePropertyInterpreter(),
            new MultiLineInterpreter(),
            new VisualPropertyInterpreter()
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
            var currentLevel = 0;

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

                if (context.BeginningOfLine)
                {
                    currentLevel = 1;
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
                }

                currentLevel--;
                var candidate = osb.ToString(currentLevel, osb.Length - currentLevel);

                var used = buffer.Start;
                var interpreter = Interpreters.FirstOrDefault(i => i.CanInterpret(candidate, context));

                if (interpreter != null)
                {
                    var usedPosition = 0;
                    try
                    {
                        var result = interpreter.Interpret(candidate, context);
                        usedPosition = result.Used;
                        if (result.Used > 0)
                        {
                            context.CurrentComponent.Add(result.Component);
                            context.Components.Push(result.Component);
                        }
                    }
                    catch (InvalidSkillFlowException invalidSkillFlow)
                    {
                        throw new InvalidSkillFlowDefinitionException(invalidSkillFlow.Message, context.LineNumber);
                    }

                    if (usedPosition == 0)
                    {
                        context.InterpretAttempts++;
                    }
                    else
                    {
                        context.InterpretAttempts = 0;
                    }

                    if (context.InterpretAttempts >= context.Options.MaximumInterpretAttempts)
                    {
                        throw new InvalidSkillFlowDefinitionException("Reached maximum interpretation attempts", context.LineNumber);
                    }


                    if (usedPosition == candidate.Length)
                    {
                        if (!readResult.IsCompleted)
                        {
                            usedPosition += context.Options.LineEnding.Length;
                        }

                        context.BeginningOfLine = true;
                    }
                    else
                    {
                        context.BeginningOfLine = false;
                    }

                    usedPosition += currentLevel;
                    used = buffer.GetPosition(usedPosition);
                }
                else
                {
                    throw new InvalidSkillFlowDefinitionException($"Unrecognised skill flow: " + candidate, context.LineNumber);
                }

                reader.AdvanceTo(used, examined);

                if (readResult.IsCompleted)
                {
                    if (context.InterpretAttempts > 0)
                    {
                        throw new InvalidSkillFlowDefinitionException($"Incomplete skill flow", context.LineNumber);
                    }
                    break;
                }
            }

            return context.Story;
        }
    }
}
