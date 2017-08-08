using System;
using System.Resources;
using Microsoft.CodeAnalysis;

namespace Kuno.CodeAnalysis
{
    public static class DiagnosticDescriptorBuilder
    {
        public static DiagnosticDescriptor GetDescriptor(string diagnosticId, string messageFormat, ResourceManager resourceManager)
        {
            return new DiagnosticDescriptor(
                diagnosticId,
                resourceManager.GetString($"{diagnosticId}_Title"),
                messageFormat,
                resourceManager.GetString($"{diagnosticId}_Category"),
                ParseSeverity(resourceManager.GetString($"{diagnosticId}_Severity")),
                bool.Parse(resourceManager.GetString($"{diagnosticId}_IsActivatedByDefault") ?? "true"),
                helpLinkUri: resourceManager.GetString($"{diagnosticId}_Url"),
                description: GetDescription(diagnosticId, resourceManager),
                customTags: "kuno");
        }

        private static string GetDescription(string diagnosticId, ResourceManager resourceManager)
        {
            return resourceManager.GetString($"{diagnosticId}_Description");
        }

        private static DiagnosticSeverity ParseSeverity(string severity)
        {
            DiagnosticSeverity result;
            if (Enum.TryParse(severity, out result))
            {
                return result;
            }

            throw new NotSupportedException($"Not supported severity");
        }
    }
}