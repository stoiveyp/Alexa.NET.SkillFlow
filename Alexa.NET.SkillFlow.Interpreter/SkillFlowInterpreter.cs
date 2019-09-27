using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.IO.Pipelines;
using System.Linq;
using System.Text;
using System.Threading;

namespace Alexa.NET.SkillFlow.Interpreter
{
    public static class SkillFlowInterpreter
    {
        public static List<ISkillFlowInterpreter> Interpreters = new List<ISkillFlowInterpreter>()
        {

        };

        public static Task<Story> Interpret(string input, CancellationToken token = default)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(input));
            return Interpret(ms, token);
        }

        public static Task<Story> Interpret(Stream input, CancellationToken token = default)
        {
            return Interpret(PipeReader.Create(input), token).AsTask();
        }

        public static async ValueTask<Story> Interpret(PipeReader reader, CancellationToken token = default)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }
            var context = new SkillFlowInterpretationContext();

            while (true)
            {
                var readResult = await reader.ReadAsync(token);
                if (readResult.Buffer.IsEmpty && readResult.IsCompleted)
                {
                    break;
                }

                foreach (var segment in readResult.Buffer)
                {
                    var candidate = Encoding.UTF8.GetString(segment.ToArray());
                    var used = Interpreters.FirstOrDefault(i => i.CanInterpret(candidate, context))?.Interpret(candidate, context) ?? 0;
                }
            }

            return context.Story;
        }
    }

    public interface ISkillFlowInterpreter
    {
        bool CanInterpret(string candidate, SkillFlowInterpretationContext context);
        int Interpret(string candidate, SkillFlowInterpretationContext context);
    }
}
