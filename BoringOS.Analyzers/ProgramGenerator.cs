using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BoringOS.Analyzers;

#nullable enable

[Generator]
public class ProgramGenerator : ISourceGenerator
{
    private const string NameofProgram = "Program";

    public void Initialize(GeneratorInitializationContext context)
    {
        // No initialization required
    }

    public void Execute(GeneratorExecutionContext context)
    {
        List<string> programNames = new();
        foreach (SyntaxTree? tree in context.Compilation.SyntaxTrees)
        {
            if (tree == null) continue;

            IEnumerable<ClassDeclarationSyntax> programs = tree.GetRoot()
                .DescendantNodes()
                .OfType<ClassDeclarationSyntax>()
                .Where(c => c.BaseList != null)
                .Where(c => c.BaseList.Types
                    .Any(t => t.Type.ToString() == NameofProgram));
            
            foreach (ClassDeclarationSyntax? program in programs)
            {
                if(program == null) continue;
                programNames.Add(program.Identifier.ValueText);
            }
        }

        string members = string.Empty;
        foreach (string program in programNames)
        {
            members += $"new {program}(), ";
        }

        string source =
            $$"""
            using BoringOS.Programs;

            namespace BoringOS;

            public abstract partial class AbstractBoringKernel
            {
                private partial List<Program> InstantiatePrograms()
                {
                    return new List<Program>({{programNames.Count}})
                    {
                        {{members}}
                    };
                }
            }
            """;
        
        context.AddSource("AbstractBoringKernel.programs.g.cs", source);
    }
}