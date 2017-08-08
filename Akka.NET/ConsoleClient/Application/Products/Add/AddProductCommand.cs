using Kuno.Validation;

namespace ConsoleClient.Application.Products.Add
{
    public class AddProductCommand
    {
        [NotNull("no")]
        public string Name { get; }

        public int Count { get; }

        public AddProductCommand(string name, int count)
        {
            this.Name = name;
            this.Count = count;
        }
    }
}