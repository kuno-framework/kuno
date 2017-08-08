using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Kuno.CodeAnalysis
{
    public static class Extensions
    {
        public static IEnumerable<INamedTypeSymbol> AllBases(this INamedTypeSymbol instance)
        {
            if (instance == null)
            {
                yield break;
            }
            if (instance.BaseType != null)
            {
                yield return instance.BaseType;
            }
            if (instance.BaseType?.Name != "Object")
            {
                foreach (var parent in instance.BaseType.AllBases())
                {
                    yield return parent;
                }
            }
        }

        public static bool IsEvent(this INamedTypeSymbol instance)
        {
            return instance.HasBase("Event");
        }

        public static bool IsExternalRequest(this INamedTypeSymbol instance)
        {
            return instance.GetAttributes().Any(e => e.AttributeClass.Name == "Request");
        }

        public static bool IsRequest(this INamedTypeSymbol instance, SymbolAnalysisContext context)
        {
            var symbols = context.Compilation.GetSymbolsWithName(e => true).OfType<INamedTypeSymbol>()
                                 .Where(e => e.BaseType != null && e.BaseType.TypeArguments.Any()).ToList();

            bool HasTypeArgument(INamedTypeSymbol symbol)
            {
                var target = symbol.BaseType.TypeArguments.FirstOrDefault();
                return target.Name == instance.Name;
            }

            return symbols.Any(e => HasTypeArgument(e)  && e.HasBase("Function"));
        }

        public static bool IsMessage(this INamedTypeSymbol instance, SymbolAnalysisContext context)
        {
            return instance.IsEvent() || instance.IsExternalRequest() || instance.IsRequest(context);
        }

        public static bool HasBase(this INamedTypeSymbol instance, string name)
        {
            return instance.AllBases().Any(e => e.Name == name);
        }

        public static bool IsMutable(this INamedTypeSymbol instance)
        {
            return instance.GetMembers().Any(e => (e as IPropertySymbol)?.SetMethod?.DeclaredAccessibility == Accessibility.Public);
        }

        public static bool HasFields(this INamedTypeSymbol instance)
        {
            return instance.GetMembers().OfType<IFieldSymbol>().Any(e => !e.Name.EndsWith("__BackingField"));
        }
    }
}