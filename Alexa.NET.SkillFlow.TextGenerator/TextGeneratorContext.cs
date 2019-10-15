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

        public async Task WriteIndent()
        {
            for (var x = 0; x < CurrentLevel; x++)
            {
                await WriteString(Indent);
            }
        }

        public async Task WriteLine(string output)
        {
            await WriteIndent();
            await WriteString(output,true);
        }

        public async Task WriteString(string output,bool lineEnding = false)
        {
            await Writer.WriteAsync(Encoding.GetBytes(output));
            if (lineEnding)
            {
                await Writer.WriteAsync(Encoding.GetBytes(LineEnding));
            }
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
