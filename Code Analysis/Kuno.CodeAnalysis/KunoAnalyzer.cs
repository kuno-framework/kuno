using Microsoft.CodeAnalysis.Diagnostics;

namespace Kuno.CodeAnalysis
{
    public abstract class KunoAnalyzer : DiagnosticAnalyzer
    {
        public override void Initialize(AnalysisContext context)
        {
            this.Initialize(new KunoAnalyzerContext(context));
        }

        public abstract void Initialize(KunoAnalyzerContext context);
    }
}