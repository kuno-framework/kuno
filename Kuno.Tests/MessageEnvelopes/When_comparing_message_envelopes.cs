using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Kuno.Tests.MessageEnvelopes
{
    public class When_comparing_message_envelopes
    {
        [Fact]
        public void should_be_equal_by_id()
        {
            var first = new MessageEnvelope("first");
            var next = new MessageEnvelope("next");
            typeof(MessageEnvelope).GetProperty("Id")?.SetValue(next, first.Id);

            first.Should().Be(next);
        }

        [Fact]
        public void should_be_equivalent_by_id()
        {
            var first = new MessageEnvelope("first");
            var next = new MessageEnvelope("next");
            typeof(MessageEnvelope).GetProperty("Id")?.SetValue(next, first.Id);

            (first == next).Should().BeTrue();
        }

        [Fact]
        public void should_be_the_same_in_lists()
        {
            var first = new MessageEnvelope("first");
            var next = new MessageEnvelope("next");
            typeof(MessageEnvelope).GetProperty("Id")?.SetValue(next, first.Id);

            var list = new List<MessageEnvelope> { first };

            list.Contains(next).Should().BeTrue();
        }
    }
}