using System;
using Kuno;
using Kuno.AzureServiceBus;
using Kuno.Serialization;
using Kuno.Services;
using Kuno.Services.Logging;
using Kuno.Services.Messaging;
using Newtonsoft.Json;

namespace ConsoleClient
{
    public class SomeEvent : Event
    {
        public string FirstName { get; set; } = "s";
    }


    [Subscribe("SomeEvent"), EndPoint("asdf")]
    public class Handler : Function
    {
        public override void Receive()
        {
            Console.WriteLine("XXX");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            using (var stack = new KunoStack())
            {
                stack.Publish("SomeEvent", "asdf");
            }
        }
    }
}