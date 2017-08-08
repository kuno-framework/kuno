using System;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Kuno.CodeAnalysis.Rules.Messages
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class MessagePropertiesMustBePublic : KunoAnalyzer
    {
        internal const string MessageFormat = "Make '{0}' public.";

        internal const string DiagnosticId = "K1002";

        public static readonly DiagnosticDescriptor Rule = DiagnosticDescriptorBuilder.GetDescriptor(DiagnosticId, MessageFormat, RuleStrings.ResourceManager);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(KunoAnalyzerContext context)
        {
            context.RegisterSymbolAction(this.AnalyzeProperty, SymbolKind.Property);
        }

        private void AnalyzeProperty(SymbolAnalysisContext context)
        {
            var target = (IPropertySymbol)context.Symbol;
            if (target.DeclaredAccessibility != Accessibility.Public && target.ContainingType.IsMessage(context))
            {
                var diagnostic = Diagnostic.Create(Rule, target.Locations[0], target.Name);
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}