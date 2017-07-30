using System;
using Autofac;
using Kuno;
using Kuno.Services;
using Kuno.Text;
using Kuno.Services.Messaging;
using Newtonsoft.Json;
using Kuno.Serialization;
using Kuno.Services.Registry;
using Kuno.Services.Services;

namespace ConsoleClient
{
    public class HelloWorldRequest
    {
        public string Name { get; set; }
    }

    public class SomeEvent : Event
    {
        public string FirstName { get; set; } = "s";
    }

    [Subscribe("SomeEvent"), EndPoint("aa", Version = 2)]
    public class R : Function<SomeEvent>
    {
        public override void Receive(SomeEvent instance)
        {
            Console.WriteLine(instance);
            Console.WriteLine("A");
        }
    }

    [Subscribe("SomeEvent2"), EndPoint("aa", Version = 3)]
    public class R2 : Function<SomeEvent>
    {
        public override void Receive(SomeEvent instance)
        {
            this.AddRaisedEvent(new SomeEvent());
            Console.WriteLine("B");
        }
    }

    class Program
    {
        public static void Main(string[] args)
        {
            using (var stack = new KunoStack())
            {

                stack.Send(new GetOpenApiRequest("localhost", all: true)).Result.OutputToJson();
            }
        }
    }
}