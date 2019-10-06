using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alexa.NET.SkillFlow.Conditions;

namespace Alexa.NET.SkillFlow.Interpreter
{
    public class ConditionContext
    {
        public StringBuilder Remaining { get; set; }
        public Stack<Value> Values { get; set; }

        public int Start { get; set; }
        public int Current { get; set; }

        public char CurrentChar => Remaining[Current];

        public void MoveNext()
        {
            Current++;
            MoveToCurrent();
        }

        public void MoveToCurrent()
        {
            Start = Current;
        }

        public Condition Condition
        {
            get
            {
                if (Values.Count == 0)
                {
                    return new ValueWrapper(new False());
                }

                if (Values.First() is Condition)
                {
                    return Values.First() as Condition;
                }

                return new ValueWrapper(Values.First());
            }
        }

        public bool Finished => Current == Remaining.Length;

        public ConditionContext(string condition)
        {
            Remaining = new StringBuilder(condition);
            Values = new Stack<Value>();
        }
    }
}