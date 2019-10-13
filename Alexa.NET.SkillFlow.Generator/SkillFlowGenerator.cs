using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Alexa.NET.SkillFlow.Generator
{
    public abstract class SkillFlowGenerator<TOutput, TContext>
    {
        protected SkillFlowGenerator(Func<TContext, TOutput> finaliser, TContext initialContext)
        {
            Context = initialContext;
            Finaliser = finaliser;
        }

        public TContext Context { get; }

        public Dictionary<Type,ISkillFlowGenerator<TContext>> Generators = new Dictionary<Type, ISkillFlowGenerator<TContext>>();

        public Func<TContext, TOutput> Finaliser { get; }

        public async Task<TOutput> Generate(Story story)
        {
            return Finaliser(await Generate(story,Context));
        }

        public Task<TContext> Generate(SkillFlowComponent component, TContext context)
        {
            Type currentType = component.GetType();
            while (currentType != null && !Generators.ContainsKey(currentType))
            {
                currentType = currentType.BaseType;
            }

            if (currentType == null)
            {
                return Task.FromResult(context);
            }

            return Generators[currentType].Generate(component, context);
        }
    }
}
