using System;
using System.Threading.Tasks;
using ConsoleClient.Application.Products.Stock;
using ConsoleClient.Domain.Products;
using Kuno.Services;

namespace ConsoleClient.Application.Products.Add
{
    [EndPoint("products/add")]
    public class AddProduct : Function<AddProductCommand, AddProductEvent>
    {
        private int _count = 0;

        public override async Task<AddProductEvent> ReceiveAsync(AddProductCommand command)
        {
            var target = new Product("name");

            Console.WriteLine("Sending with item number of " + ++_count);
            if (_count > 2)
            {
                throw new Exception("The current count is greater than 2.");
            }

            await this.Domain.Add(target);

            var stock = await this.Send(new StockProductCommand(command.Count));
            if (!stock.IsSuccessful)
            {
                await this.Domain.Remove(target);

                throw new DependencyFailedException(this.Request, stock);
            }

            //var remote = await this.Send("remote", null);
            //if (!remote.IsSuccessful)
            //{
            //    throw new ChainFailedException(this.Request, remote);
            //}

            return new AddProductEvent();
        }
    }
}