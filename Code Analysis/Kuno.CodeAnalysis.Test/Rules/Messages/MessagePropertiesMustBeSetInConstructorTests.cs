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
    public class MessagePropertiesMustBeSetInConstructorTests : CodeFixVerifier
    {

        [TestMethod]
        public void Valid()
        {
            var test = @"using System;
using Kuno.Services;

namespace Test
{
    public class AddRequest
    {
        public string Property { get; }

        public AddRequest(string property)
        {
            this.Property = property;
        }
    }

    public class Add : EndPoint<AddRequest>
    {
        public override void Receive(AddRequest instance)
        {
            base.Receive(instance);
        }
    }
}";
            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void Invalid()
        {
            var test = @"using System;
using Kuno.Services;

namespace Test
{
    public class AddRequest
    {
        public string Property { get; }
    }

    public class Add : EndPoint<AddRequest>
    {
        public override void Receive(AddRequest instance)
        {
            base.Receive(instance);
        }
    }
}";
            var expected = new DiagnosticResult
            {
                Id = MessagePropertiesMustBeSetInConstructor.Rule.Id,
                Message = String.Format(MessagePropertiesMustBeSetInConstructor.Rule.MessageFormat.ToString(), "Property"),
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[] {
                        new DiagnosticResultLocation("Test.cs", 8, 23)
                    }
            };

            VerifyCSharpDiagnostic(test, expected);
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new MessagePropertiesMustBeSetInConstructor();
        }
    }
}