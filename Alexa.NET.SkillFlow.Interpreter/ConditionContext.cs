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

        public string CurrentWord => Remaining.ToString(Start, Current - Start);
        public char? NextChar => Start >= Remaining.Length ? (char?)null : Remaining[Start];

        public void MoveCurrent(int number = 1)
        {
            if (Current >= Remaining.Length)
            {
                Finished = true;
                return;
            }
            Current += number;
        }

        public void MoveNext(int number = 1)
        {
            MoveCurrent(number);
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

        public bool Finished { get; set; }
    
        public char? Peek => Finished || Start >= Remaining.Length-1 ? (char?)null : Remaining[Start+1];
        public char? PeekCurrent => Finished || Current >= Remaining.Length - 1 ? (char?) null : Remaining[Current+1];

        public ConditionContext(string condition)
        {
            Remaining = new StringBuilder(condition);
            Values = new Stack<Value>();
        }

        public void Push(Value value)
        {
            Values.Push(value);
        }

        public bool PeekWord(string word)
        {
            return Start + word.Length <= Remaining.Length && Remaining.ToString(Start, word.Length) == word;
        }
    }
}