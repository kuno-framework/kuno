using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestHelper;
using Kuno.CodeAnalysis;
using Kuno.CodeAnalysis.Rules.Messages;

namespace Kuno.CodeAnalysis.Test
{
    [TestClass]
    public class MessagePropertiesMustBeReadOnlyTests : CodeFixVerifier
    {

        [TestMethod]
        public void MessagesWithPropertiesShouldBreakRule()
        {
            var test = @"using System;

namespace Test
{
    public class AddRequest
    {
        public string Property { get; set; }
    }

    public class Add : EndPoint<AddRequest>
    {
        public override void Execute(AddCommand command)
        {
        }
    }
}";
            var expected = new DiagnosticResult
            {
                Id = MessagePropertiesMustBeReadOnly.Rule.Id,
                Message = String.Format(MessagePropertiesMustBeReadOnly.Rule.MessageFormat.ToString(), "Property"),
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[] {
                        new DiagnosticResultLocation("Test.cs", 7, 23)
                    }
            };

            VerifyCSharpDiagnostic(test, expected);
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new MessagePropertiesMustBeReadOnly();
        }
    }
}