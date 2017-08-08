using System;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Kuno.CodeAnalysis
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class RequestsShouldEndInRequest : DiagnosticAnalyzer
    {
        internal const string MessageFormat = "The request type '{0}' should end in 'Request'.";

        internal const string DiatnosticId = "K1100";

        public static readonly DiagnosticDescriptor Rule = DiagnosticDescriptorBuilder.GetDescriptor(DiatnosticId,
            MessageFormat, RuleStrings.ResourceManager);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSymbolAction(this.AnalyzeField, SymbolKind.NamedType);
        }

        private void AnalyzeField(SymbolAnalysisContext context)
        {
            var target = (INamedTypeSymbol)context.Symbol;
            if (target.IsRequest(context) && !(target.Name.EndsWith("Request") || target.Name.EndsWith("Command") || target.Name.EndsWith("Query")))
            {
                var diagnostic = Diagnostic.Create(Rule, target.Locations[0], target.Name);

                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}