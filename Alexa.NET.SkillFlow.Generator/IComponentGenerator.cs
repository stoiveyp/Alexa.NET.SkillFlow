using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Alexa.NET.SkillFlow.Generator
{
    public interface IComponentGenerator<TContext>
    {
        Task<TContext> Generate(SkillFlowComponent component, TContext context);
    }
}
