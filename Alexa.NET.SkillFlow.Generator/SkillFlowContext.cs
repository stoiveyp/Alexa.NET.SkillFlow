using System;
using System.Collections.Generic;
using System.Text;

namespace Alexa.NET.SkillFlow.Generator
{
    public abstract class SkillFlowContext
    {
        protected Stack<(int,int)> Loops { get; } = new Stack<(int, int)>();

        public void SetLoop(int index, int max)
        {
            Loops.Push((index,max));
        }

        public void ClearLoop()
        {
            Loops.Pop();
        }

        public bool IsFirst => Loops.Peek().Item1 == 0;
        public bool IsLast => Loops.Peek().Item1 == Loops.Peek().Item2;
        public int Index => Loops.Peek().Item1;
    }
}
