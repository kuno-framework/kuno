using System;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Kuno.CodeAnalysis
{
    public class KunoAnalyzerContext
    {
        private readonly AnalysisContext context;
        public static Func<SyntaxTree, bool> ShouldAnalysisBeDisabled { get; set; }

        internal KunoAnalyzerContext(AnalysisContext context)
        {
            this.context = context;
        }

        internal void RegisterCodeBlockAction(Action<CodeBlockAnalysisContext> action)
        {
            context.RegisterCodeBlockAction(
                c =>
                {
                    if (IsAnalysisDisabled(c.CodeBlock.SyntaxTree, null))
                    {
                        return;
                    }

                    action(c);
                });
        }

        internal void RegisterCodeBlockStartAction<TLanguageKindEnum>(Action<CodeBlockStartAnalysisContext<TLanguageKindEnum>> action)
            where TLanguageKindEnum : struct
        {
            context.RegisterCodeBlockStartAction<TLanguageKindEnum>(
                c =>
                {
                    if (IsAnalysisDisabled(c.CodeBlock.SyntaxTree, c.SemanticModel.Compilation))
                    {
                        return;
                    }

                    action(c);
                });
        }

        internal void RegisterCompilationAction(Action<CompilationAnalysisContext> action)
        {
            context.RegisterCompilationAction(
                c =>
                {
                    if (IsAnalysisDisabled(c.Compilation.SyntaxTrees.FirstOrDefault(), c.Compilation))
                    {
                        return;
                    }

                    action(c);
                });
        }

        public void RegisterCompilationStartAction(Action<CompilationStartAnalysisContext> action)
        {
            context.RegisterCompilationStartAction(
                c =>
                {
                    if (IsAnalysisDisabled(c.Compilation.SyntaxTrees.FirstOrDefault(), c.Compilation))
                    {
                        return;
                    }

                    action(c);
                });
        }

        internal void RegisterSemanticModelAction(Action<SemanticModelAnalysisContext> action)
        {
            context.RegisterSemanticModelAction(
                c =>
                {
                    if (IsAnalysisDisabled(c.SemanticModel.SyntaxTree, null))
                    {
                        return;
                    }

                    action(c);
                });
        }

        internal void RegisterSyntaxNodeAction<TLanguageKindEnum>(Action<SyntaxNodeAnalysisContext> action,
                                                                  ImmutableArray<TLanguageKindEnum> syntaxKinds)
            where TLanguageKindEnum : struct
        {
            this.RegisterSyntaxNodeAction(action, syntaxKinds.ToArray());
        }

        internal void RegisterSyntaxNodeAction<TLanguageKindEnum>(Action<SyntaxNodeAnalysisContext> action,
                                                                  params TLanguageKindEnum[] syntaxKinds)
            where TLanguageKindEnum : struct
        {
            context.RegisterSyntaxNodeAction(
                c =>
                {
                    if (IsAnalysisDisabled(c.Node.SyntaxTree, null))
                    {
                        return;
                    }

                    action(c);
                },
                syntaxKinds);
        }

        internal void RegisterSyntaxTreeAction(Action<SyntaxTreeAnalysisContext> action)
        {
            context.RegisterSyntaxTreeAction(
                c =>
                {
                    if (IsAnalysisDisabled(c.Tree, null))
                    {
                        return;
                    }

                    action(c);
                });
        }

        internal void RegisterSymbolAction(Action<SymbolAnalysisContext> action, ImmutableArray<SymbolKind> symbolKinds)
        {
            this.RegisterSymbolAction(action, symbolKinds.ToArray());
        }

        public void RegisterSymbolAction(Action<SymbolAnalysisContext> action, params SymbolKind[] symbolKinds)
        {
            context.RegisterSymbolAction(
                c =>
                {
                    if (IsAnalysisDisabled(c.Symbol.Locations.FirstOrDefault(l => l.SourceTree != null)?.SourceTree, c.Compilation))
                    {
                        return;
                    }

                    action(c);
                },
                symbolKinds);
        }

        internal static bool IsAnalysisDisabled(SyntaxTree tree, Compilation compilation)
        {
            if (compilation != null && !compilation.ReferencedAssemblyNames.Any(e => e.Name.Contains("Kuno")))
            {
                return false;
            }

            if (ShouldAnalysisBeDisabled == null)
            {
                return false;
            }

            return tree != null && ShouldAnalysisBeDisabled(tree);
        }
    }
}