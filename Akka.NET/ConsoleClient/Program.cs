using System;
using System.Threading.Tasks;
using Akka.Util;
using ConsoleClient.Application.Products.Add;
using Kuno;
using Kuno.Akka;
using Kuno.Akka.EndPoints;
using Kuno.Akka.Messaging;
using Kuno.Services;

#pragma warning disable 4014

namespace ConsoleClient
{
    //[EndPointHost("home/parent")]
    //public class AC : EndPointHost
    //{
    //    public override int Retries => 2;
    //}

    

    [EndPoint("go")]
    public class Parent : Function
    {
        public override async Task ReceiveAsync()
        {
            await Task.Delay(1000);

            Console.WriteLine("Hello");
        }
    }

    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Console.Title = "Console Client";

            try
            {
                using (var stack = new KunoStack())
                {
                    stack.UseAkka();

                    for (int i = 0; i < 10; i++)
                    {
                        stack.Send("go");
                    }

                    stack.Shutdown().Wait();
                    Console.ReadKey();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}