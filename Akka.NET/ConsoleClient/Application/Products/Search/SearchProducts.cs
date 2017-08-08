using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kuno.Services;

namespace ConsoleClient.Application.Products.Search
{
    public class SearchProductsCommand
    {
    }

    [EndPoint("products/search")]
    public class SearchProducts : Function<SearchProductsCommand, string>
    {
        public override async Task<string> ReceiveAsync(SearchProductsCommand command)
        {
            await Task.Delay(500);

            return "adsfaf";
        }
    }
}
