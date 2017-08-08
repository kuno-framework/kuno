using System;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Kuno.CodeAnalysis.Rules.Messages
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class MessagePropertiesMustBeSetInConstructor : KunoAnalyzer
    {
        internal const string MessageFormat = "Initialize '{0}' in the constructor.";

        internal const string DiagnosticId = "K1003";

        public static readonly DiagnosticDescriptor Rule = DiagnosticDescriptorBuilder.GetDescriptor(DiagnosticId, MessageFormat, RuleStrings.ResourceManager);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(KunoAnalyzerContext context)
        {
            context.RegisterSymbolAction(this.AnalyzeProperty, SymbolKind.Property);
        }

        private void AnalyzeProperty(SymbolAnalysisContext context)
        {
            var target = (IPropertySymbol)context.Symbol;
            var parent = target.ContainingType;
            bool contains = false;
            foreach (var method in parent.Constructors.OfType<IMethodSymbol>())
            {
                var syntax = method.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntax();
                if (syntax != null)
                {
                    foreach (var item in syntax.DescendantNodes().Where(m => m.IsKind(Microsoft.CodeAnalysis.CSharp.SyntaxKind.SimpleAssignmentExpression)))
                    {
                        var current = item.DescendantNodes().FirstOrDefault(ll => ll.IsKind(Microsoft.CodeAnalysis.CSharp.SyntaxKind.IdentifierName) && ((IdentifierNameSyntax)ll).Identifier.Value == target.Name);
                        if (current != null)
                        {
                            contains = true;
                            break;
                        }
                    }
                }
            }
            if (!contains)
            {
                var diagnostic = Diagnostic.Create(Rule, target.Locations[0], target.Name);
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}