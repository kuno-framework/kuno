using System;
using System.Text;
using Kuno;
using Kuno.AspNetCore;
using Kuno.Security;
using Kuno.Services;

namespace ConsoleHost
{
    /// <summary>
    /// asdfsadfaf
    /// </summary>
    public class HelloWorldRequest
    {
        /// <summary>
        /// asdfas
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// Some summary here.
    /// </summary>
    [EndPoint("hello/greet-me", Method = "GET")]
    public class HelloWorld : Function<HelloWorldRequest, string>
    {
        public override string Receive(HelloWorldRequest instance)
        {
            return "Hello " + instance.Name + "!";
        }
    }

    [EndPoint("session")]
    public class GetSession : Function
    {
        public override void Receive()
        {
            this.Respond(this.Request.SessionId);
        }
    }

    public class Go
    {
    }

    class Program
    {
        static void Main(string[] args)
        {
            using (var stack = new KunoStack())
            {
                stack.RunWebHost();
            }
        }
    }
}