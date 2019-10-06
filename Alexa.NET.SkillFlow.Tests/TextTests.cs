using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alexa.NET.SkillFlow.Interpreter;
using Xunit;

namespace Alexa.NET.SkillFlow.Tests
{
    public class TextTests
    {
        [Fact]
        public void InterpreterValidOnNewLine()
        {
            var interpreter = new MultiLineInterpreter();
            var context = new SkillFlowInterpretationContext(new SkillFlowInterpretationOptions());
            context.Components.Push(new Text("nocare"));
            Assert.True(interpreter.CanInterpret(string.Empty, context));
        }


        [Fact]
        public void TextTypeSetCorrectly()
        {
            var testText = "test text";
            var text = new Text(testText);
            Assert.Equal(testText, text.TextType);
        }

        [Fact]
        public void AddTextLineSetsFirstContent()
        {
            var testtext = "wibble";
            var text = new Text("dontcare");
            var line = new TextLine(testtext);
            text.Add(line);
            var content = Assert.Single(text.Content);
            Assert.Equal(testtext, content);
        }

        [Fact]
        public void AddTextLineTwiceAppendsFirstContent()
        {
            var testtext = "wibble";
            var text = new Text("dontcare");
            var line = new TextLine(testtext);
            text.Add(line);
            text.Add(line);
            var content = Assert.Single(text.Content);
            Assert.Equal(testtext + testtext, content);
        }

        [Fact]
        public void InterpreterUnderstandsVariation()
        {
            var interpreter = new MultiLineInterpreter();
            var result = interpreter.Interpret("||", new SkillFlowInterpretationContext(new SkillFlowInterpretationOptions()));
            Assert.IsType<Variation>(result.Component);
        }

        [Fact]
        public void AddVariationAddsSecondOption()
        {
            var testtext = "wibble";
            var text = new Text("dontcare");
            var line = new TextLine(testtext);
            text.Add(line);
            text.Add(new Variation());
            text.Add(line);
            text.Add(line);
            Assert.Equal(2,text.Content.Count);
            Assert.Equal(testtext, text.Content.First());
            Assert.Equal(testtext + testtext, text.Content.Skip(1).First());
        }
    }
}
