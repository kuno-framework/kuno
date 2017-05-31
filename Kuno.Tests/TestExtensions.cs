using System.Linq;
using FluentAssertions;
using Kuno.Services.Inventory;
using Kuno.Services.Messaging;
using Kuno.Validation;

namespace Kuno.Tests
{
    public static class TestExtensions
    {
        public static void ShouldBeSecure(this EndPointMetaData endpoint)
        {
            endpoint.Rules.Should().Contain(e => e.RuleType == ValidationType.Security);
        }

        public static void ShouldBeSuccessful(this MessageResult result, string because = "message execution should have been successful")
        {
            result.IsSuccessful.Should().BeTrue(because);
        }

        public static void ShouldNotBeSuccessful(this MessageResult result, string because = "message execution should not have been successful")
        {
            result.IsSuccessful.Should().BeFalse(because);
        }

        public static void ShouldContainMessage(this MessageResult result, string message, string because = "message execution should have returned a validation message containing {0}", params object[] becauseArgs)
        {
            if (becauseArgs.Length == 0 && because.Contains("{0}"))
            {
                becauseArgs = new object[] { message };
            }
            result.ValidationErrors.Select(e => e.Message).Should().Contain(message, because, becauseArgs);
        }

        public static void ShouldContainMessage(this MessageResult result, ValidationType type, string message, string because = "message execution should have returned a validation message containing {0} of type {1}", params object[] becauseArgs)
        {
            if (becauseArgs.Length < 2 && because.Contains("{1}"))
            {
                becauseArgs = new object[] { message , type };
            }
            result.ValidationErrors.Should().Contain(e => e.Type == type && e.Message == message, because, becauseArgs);
        }
    }
}
