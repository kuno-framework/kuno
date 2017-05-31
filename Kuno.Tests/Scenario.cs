using Kuno.Domain;

namespace Kuno.Tests
{
    public class Scenario
    {
        public InMemoryEntityContext EntityContext { get; set; } = new InMemoryEntityContext();

        public Scenario WithData(params IAggregateRoot[] items)
        {
            this.EntityContext.Add(items).Wait();

            return this;
        }
    }
}