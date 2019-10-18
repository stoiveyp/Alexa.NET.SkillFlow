using System;
using System.IO;
using System.Threading.Tasks;
using Alexa.NET.SkillFlow.CodeGenerator;

namespace Alexa.NET.SkillFlow.CodeOutput
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var context = new CodeGeneratorContext();
            var directory = Directory.CreateDirectory("test");
            await context.Output(directory);
        }
    }
}
