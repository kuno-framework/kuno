using System;
using System.Collections.Immutable;
using Kuno.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Kuno.CodeAnalysis.Rules.Messages
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class MessagePropertiesMustBeReadOnly : KunoAnalyzer
    {
        internal const string MessageFormat = "Make '{0}' read-only.";

        internal const string DiagnosticId = "K1001";

        public static readonly DiagnosticDescriptor Rule = DiagnosticDescriptorBuilder.GetDescriptor(DiagnosticId, MessageFormat, RuleStrings.ResourceManager);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(KunoAnalyzerContext context)
        {
            context.RegisterSymbolAction(this.AnalyzeProperty, SymbolKind.Property);
        }

        private void AnalyzeProperty(SymbolAnalysisContext context)
        {
            var target = (IPropertySymbol)context.Symbol;
            if (target.SetMethod != null && target.SetMethod.DeclaredAccessibility != Accessibility.Private && target.ContainingType.IsMessage(context))
            {
                var diagnostic = Diagnostic.Create(Rule, target.Locations[0], target.Name);
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}