using System;
using Kuno;
using Kuno.Services;
using Kuno.Text;
using Kuno.Services.Messaging;
using Newtonsoft.Json;
using Kuno.Serialization;

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

    public class R : Function<SomeEvent>
    {
        public override void Receive(SomeEvent instance)
        {
            Console.WriteLine(instance);
        }
    }

    class Program
    {
        public static void Main(string[] args)
        {
            using (var stack = new KunoStack())
            {
                stack.Publish(new SomeEvent());
            }
        }
    }
}