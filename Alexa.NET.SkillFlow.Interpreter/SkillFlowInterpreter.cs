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
        private SkillFlowInterpretationOptions _options;

        public SkillFlowInterpreter(SkillFlowInterpretationOptions options,
            params ISkillFlowInterpreter[] customInterpreters)
        {
            Interpreters.AddRange(customInterpreters);
            _options = options ?? new SkillFlowInterpretationOptions();
        }

        public SkillFlowInterpreter(params ISkillFlowInterpreter[] customInterpreters):this(new SkillFlowInterpretationOptions(),customInterpreters)
        {
            
        }

        public List<ISkillFlowInterpreter> Interpreters = new List<ISkillFlowInterpreter>()
        {
            new SceneInterpreter()
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

                var containsLineBreak = false;
                var first = true;
                foreach (var segment in buffer)
                {
                    var segmentString = Encoding.UTF8.GetString(segment.ToArray());
                    if (first)
                    {
                        first = false;
                        osb.Append(segmentString.TrimStart());
                    }
                    else
                    {
                        osb.Append(segmentString);
                    }

                    if (segmentString.Contains(_options.LineEnding))
                    {
                        containsLineBreak = true;
                        break;
                    }
                }

                var candidate = osb.ToString();
                if (!readResult.IsCompleted && !containsLineBreak)
                {
                    reader.AdvanceTo(buffer.Start, buffer.End);
                    continue;
                }

                var examined = buffer.End;
                if (containsLineBreak)
                {
                    var cutoff = candidate.IndexOf(context.Options.LineEnding);
                    candidate = candidate.Substring(0, cutoff);
                    examined = buffer.GetPosition(cutoff);
                }

                context.LineNumber++;

                var used = buffer.Start;
                var interpreter = Interpreters.FirstOrDefault(i => i.CanInterpret(candidate, context));

                if (interpreter != null)
                {
                    var usedPosition = interpreter.Interpret(candidate, context);

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
                        throw new InvalidSkillFlowException("Reached maximum interpretation attempts",context.LineNumber);
                    }


                    if (containsLineBreak && usedPosition == candidate.Length)
                    {
                        usedPosition += context.Options.LineEnding.Length;
                    }

                    used = buffer.GetPosition(usedPosition);
                }
                else
                {
                    throw new InvalidSkillFlowException($"Unable to process skill flow", context.LineNumber);
                }

                reader.AdvanceTo(used, examined);

                if (readResult.IsCompleted)
                {
                    if (context.InterpretAttempts > 0)
                    {
                        throw new InvalidSkillFlowException($"Incomplete skill flow", context.LineNumber);
                    }
                    break;
                }
            }

            return context.Story;
        }
    }
}
