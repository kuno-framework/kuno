using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Autofac;
using Kuno;
using Kuno.Security;
using Kuno.Services;
using Kuno.Text;
using Kuno.Services.Messaging;
using Newtonsoft.Json;
using Kuno.Serialization;
using Kuno.Services.OpenApi;
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

    [Subscribe("SomeEvent"), EndPoint("aa-a-a/adf", Version = 2)]
    public class R : Function<SomeEvent>
    {
        public override void Receive(SomeEvent instance)
        {
            Task.Delay(1000).Wait();

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

    [EndPoint("abc")]
    public class R3 : Function
    {
        public override void Receive()
        {
            Console.WriteLine("ADSF");
        }
    }

    class Program
    {
        public static void Main(string[] args)
        {
            using (var stack = new KunoStack())
            {
                stack.Send(new SomeEvent());

                stack.Shutdown().Wait();

                stack.GetRequests().OutputToJson();
            }

        }
    }
}