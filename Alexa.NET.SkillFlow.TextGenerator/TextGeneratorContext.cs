using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipelines;
using System.Text;
using System.Threading.Tasks;

namespace Alexa.NET.SkillFlow.TextGenerator
{
    public class TextGeneratorContext
    {
        public PipeWriter Writer { get; set; }

        public int CurrentLevel { get; set; }

        public Encoding Encoding { get; set; } = Encoding.UTF8;

        public string LineEnding { get; set; } = Environment.NewLine;

        public string Indent { get; set; } = "\t";

        public async Task WriteLine(string output)
        {
            for (var x = 0; x < CurrentLevel; x++)
            {
                await WriteString(Indent);

            }
            await WriteString(output);
            await WriteString(LineEnding);
        }

        protected ValueTask<FlushResult> WriteString(string output)
        {
            return Writer.WriteAsync(Encoding.GetBytes(output));
        }

        public TextGeneratorContext(PipeWriter writer)
        {
            Writer = writer;
        }

        public TextGeneratorContext(Stream stream):this(PipeWriter.Create(stream))
        {
            
        }
    }
}
