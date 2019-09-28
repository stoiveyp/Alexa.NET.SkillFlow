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
    public static class SkillFlowInterpreter
    {
        public static List<ISkillFlowInterpreter> Interpreters = new List<ISkillFlowInterpreter>()
        {
            new SceneInterpreter()
        };

        public static Task<Story> Interpret(string input, CancellationToken token = default)
        {
            return Interpret(input, new SkillFlowInterpretationOptions(), token);
        }

        public static Task<Story> Interpret(string input, SkillFlowInterpretationOptions options, CancellationToken token = default)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(input));
            return Interpret(ms, options, token);
        }

        public static Task<Story> Interpret(Stream input, CancellationToken token = default)
        {
            return Interpret(input, new SkillFlowInterpretationOptions(), token);
        }

        public static Task<Story> Interpret(Stream input, SkillFlowInterpretationOptions options, CancellationToken token = default)
        {
            return Interpret(PipeReader.Create(input), options, token).AsTask();
        }

        public static ValueTask<Story> Interpret(PipeReader reader, CancellationToken token = default)
        {
            return Interpret(reader, new SkillFlowInterpretationOptions(), token);
        }

        public static async ValueTask<Story> Interpret(PipeReader reader, SkillFlowInterpretationOptions options, CancellationToken token = default)
        {
            options = options ?? new SkillFlowInterpretationOptions();

            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            var context = new SkillFlowInterpretationContext(options);
            var osb = new StringBuilder();

            while (true)
            {
                var readResult = await reader.ReadAsync(token);
                var buffer = readResult.Buffer;
                if (buffer.IsEmpty && readResult.IsCompleted)
                {
                    break;
                }

                var containsLineBreak = false;
                foreach (var segment in buffer)
                {
                    var segmentString = Encoding.UTF8.GetString(segment.ToArray());
                    osb.Append(segmentString);
                    if (segmentString.Contains(options.LineEnding))
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
                    break;
                }
            }

            return context.Story;
        }
    }
}
