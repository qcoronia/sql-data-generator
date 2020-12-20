using System;
using System.Threading.Tasks;

using generator_operations;

namespace sql_data_generator
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await Generator.GenerateAsync(

                //ItemBinCardOperation.GenerateAsync,
                //StockTransferOperation.GenerateAsync,
                InvoiceOperation.GenerateAsync,
                context => Task.CompletedTask
            );
        }
    }
}
