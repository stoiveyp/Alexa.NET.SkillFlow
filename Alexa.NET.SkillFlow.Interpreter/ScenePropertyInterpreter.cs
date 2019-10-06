﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alexa.NET.SkillFlow.Interpreter
{
    public class ScenePropertyInterpreter : ISkillFlowInterpreter
    {
        readonly string[] TextWords = { "recap", "say", "reprompt" };
        public bool CanInterpret(string candidate, SkillFlowInterpretationContext context)
        {
            var property = candidate.Substring(1);
            return candidate[0] == '*' &&
                   (TextWords.Contains(property)
                    || property == "show"
                    || property == "then");


        }

        public InterpreterResult Interpret(string candidate, SkillFlowInterpretationContext context)
        {
            var property = candidate.Substring(1);
            switch (property)
            {
                case "show":
                    return new InterpreterResult(candidate.Length,new Visual());
                case "then":
                    return new InterpreterResult(candidate.Length,new SceneInstructions());
                default:
                    return new InterpreterResult(candidate.Length, new Text(candidate.Substring(1)));
            }
        }
    }
}
