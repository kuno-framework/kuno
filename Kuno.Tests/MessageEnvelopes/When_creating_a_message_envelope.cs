using System;
using FluentAssertions;
using Xunit;

namespace Kuno.Tests.MessageEnvelopes
{
    public class When_creating_a_message_envelope
    {
        [Fact]
        public void should_raise_exception_with_null_body()
        {
            var action = new Action(() => new MessageEnvelope(null));

            action.ShouldThrow<ArgumentException>();
        }
    }
}