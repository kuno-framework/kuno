using System;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Kuno.CodeAnalysis
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class MessagesCannotHaveFields : KunoAnalyzer
    {
        internal const string MessageFormat = "Remove field '{0}'.";

        internal const string DiatnosticId = "K1000";

        public static readonly DiagnosticDescriptor Rule = DiagnosticDescriptorBuilder.GetDescriptor(DiatnosticId,
            MessageFormat, RuleStrings.ResourceManager);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(KunoAnalyzerContext context)
        {
            context.RegisterSymbolAction(this.AnalyzeField, SymbolKind.Field);
        }

        private void AnalyzeField(SymbolAnalysisContext context)
        {
            var target = (IFieldSymbol)context.Symbol;
            if (target.ContainingType.IsMessage(context))
            {
                var diagnostic = Diagnostic.Create(Rule, target.Locations[0], target.ContainingType.Name);

                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}