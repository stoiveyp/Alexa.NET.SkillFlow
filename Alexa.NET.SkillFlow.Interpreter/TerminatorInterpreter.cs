using System;
using System.Collections.Generic;
using System.Text;
using Alexa.NET.SkillFlow.Terminators;

namespace Alexa.NET.SkillFlow.Interpreter
{
    public class TerminatorInterpreter:ISkillFlowInterpreter
    {
        public bool CanInterpret(string candidate, SkillFlowInterpretationContext context)
        {
            return candidate.StartsWith(">> ");
        }

        public InterpreterResult Interpret(string candidate, SkillFlowInterpretationContext context)
        {
            var pieces = candidate.Split(new []{' '}, 2, StringSplitOptions.RemoveEmptyEntries);
            switch (pieces[1].ToLower())
            {
                case "restart":
                    return new InterpreterResult(new Restart());
                case "pause":
                    return new InterpreterResult(new Pause());
                case "resume":
                    return new InterpreterResult(new Resume());
                case "repeat":
                    return new InterpreterResult(new Repeat());
                case "reprompt":
                    return new InterpreterResult(new Reprompt());
                case "back":
                    return new InterpreterResult(new Back());
                case "end":
                    return new InterpreterResult(new End());
                case "return":
                    return new InterpreterResult(new Return());
            }
            throw new InvalidSkillFlowDefinitionException($"Unknown terminator {pieces[1]}", context.LineNumber);
        }
    }
}
