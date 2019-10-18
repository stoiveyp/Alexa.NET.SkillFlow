using System.IO;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Alexa.NET.SkillFlow.Generator;

namespace Alexa.NET.SkillFlow.CodeGenerator
{
    public class CodeGeneratorContext:SkillFlowContext
    {
        public CodeGeneratorContext()
        {
            
        }

        public Task Output(DirectoryInfo directory)
        {
            //Needs to be AdHoc Workspacei
            var workspace = Workspace.LoadStandAloneProject(directory.FullName + Path.DirectorySeparatorChar + "test.sln");
            var solution = workspace.CurrentSolution;
            var newSolution = solution;

            foreach (var project in solution.Projects)
            {
                foreach (var document in project.Documents)
                {
                    if (document.LanguageServices.Language == LanguageNames.CSharp)
                    {
                        var tree = document.GetSyntaxTree().Root as SyntaxNode;
                        var newTree = tree.Deregionize();

                        if (tree != newTree)
                        {
                            var newText = newTree.GetFullTextAsIText();
                            newSolution = newSolution.UpdateDocument(document.Id, newText);
                        }
                    }
                }
            }

            if (newSolution != solution)
            {
                workspace.ApplyChanges(solution, newSolution);
            }

        }
    }
}
