using System;
using System.Collections.Generic;
using System.Linq;
using Kuno.Services.Validation;
using Kuno.Validation;

namespace ConsoleClient.Application.Products.Stock.Rules
{
    public class stock_count_must_be_greater_than_zero : BusinessRule<StockProductCommand>
    {
        public override ValidationError Validate(StockProductCommand instance)
        {
            if (instance.ItemCount < 5)
            {
                return "The stock count must be in multiples of 5.";
            }

            return null;
        }
    }
}
