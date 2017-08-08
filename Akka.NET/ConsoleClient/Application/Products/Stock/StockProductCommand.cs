
namespace ConsoleClient.Application.Products.Stock
{
    public class StockProductCommand
    {
        public int ItemCount { get; }

        public StockProductCommand(int itemCount) 
        {
            this.ItemCount = itemCount;
        }
    }
}