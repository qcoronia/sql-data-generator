using System;
using System.Threading.Tasks;

using generator_operations;

namespace sql_data_generator
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // We add operations here in the same manner
            // CustomerOperation.GenerateAsync,
            // ProductOperation.GenerateAsync,
            // ...

            await Generator.GenerateAsync(
                InvoiceOperation.GenerateAsync,
                context => Task.CompletedTask
            );
        }
    }
}
