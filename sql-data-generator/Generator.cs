using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accelerate.DataLayer;
using Microsoft.EntityFrameworkCore;

namespace sql_data_generator
{
    public class Generator
    {
        public static async Task GenerateAsync(params Func<MyDbContext, Task>[] operations)
        {
            Console.WriteLine($"Creating context");
            MyDbContext context = CreateDbContext();

            //var trans = await context.Database.BeginTransactionAsync();

            foreach (var operation in operations)
            {
                await operation(context);
            }

            Console.WriteLine($"Saving");
            try
            {
                await context.SaveChangesAsync();
            }
            catch (Exception)
            {
                Console.WriteLine($"Error occurred");

                //await trans.RollbackAsync();
                throw;
            }

            Console.WriteLine($"Completed");

            //await trans.CommitAsync();
        }

        private static MyDbContext CreateDbContext()
        {
            var connectionString = "<connection string>";
            var optionsBuilder = new DbContextOptionsBuilder<MyDbContext>();
            optionsBuilder.UseLazyLoadingProxies();
            optionsBuilder.UseSqlServer(connectionString, options =>
            {
                options.EnableRetryOnFailure();
                options.CommandTimeout(180); // 180 secs <== 3 mins
            });

            var context = new MyDbContext(optionsBuilder.Options);
            return context;
        }
    }
}
