using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using generator_tools;
using Microsoft.EntityFrameworkCore;

// reference to db entity models
// using MyProject.DbEntities;

namespace generator_operations
{
    public class InvoiceOperation
    {
        public static async Task GenerateAsync(MyDbContext context)
        {
            const int QTY_LIMIT = 1_000;
            const int ITM_LIMIT = 20;
            string[] STATUSES = new string[] { "PENDING", "APPROVED" };

            var quantity = 1_000;

            Console.WriteLine($"Loading requirements");
            var statuses = context.TransactionStatuses.AsEnumerable()
                .Where(e => STATUSES.Any(f => f == e.Flag));
            var salesTerms = context.SalesTerms.AsEnumerable();

            var data = new List<Invoice>();

            for (int i = 0; i < quantity; i++)
            {
                Console.CursorLeft = 0;
                Console.CursorTop -= 1;
                Console.WriteLine();

                Console.CursorLeft = 0;
                Console.CursorTop -= 1;
                Console.WriteLine($"Generated : {i + 1} of {quantity}");

                var status = Randomizer.PickFrom(statuses);
                var salesTerm = Randomizer.PickFrom(salesTerms);
                var items = context.Items.OrderBy(e => Guid.NewGuid()).Take(Randomizer.GenerateInt(1, ITM_LIMIT));
                var customer = await context.Customer.OrderBy(e => Guid.NewGuid()).FirstOrDefaultAsync();

                var newEntity = new Invoice
                {
                    CustomerId = customer.Id,
                    SalesTermsId = salesTerm.Id,
                    Transaction = new Transaction
                    {
                        TransactionStatusId = status.Id,
                        TransactionReference = "INV" + Randomizer.GenerateNumberString(9),
                        TransactionMemo1 = Randomizer.GenerateNaturalWord(200, includeSpaces: true, casing: StringCasing.Title),
                        ProcessDate = Randomizer.GenerateDate().ToUniversalTime(),
                        ValueDate = Randomizer.GenerateDate().ToUniversalTime(),
                        DueDate = Randomizer.GenerateDate().ToUniversalTime(),
                        Ext = new List<TransactionExt>
                        {
                            new TransactionExt
                            {
                                Flag = "SHIPPING_DATE",
                                DateValue = Randomizer.GenerateDate(days: 60).ToUniversalTime(),
                            },
                        },
                    },
                    Lines = items.Select(item => new InvoiceLine
                    {
                        ItemId = item.Id,
                        SerialNumber = Randomizer.GenerateString(3, StringCasing.Upper) + Randomizer.GenerateNumberString(9),
                        Price = item.Price ?? 0M,
                    }).ToList(),
                };

                foreach (var detail in newEntity.InvoiceDetail)
                {
                    var isRatePercent = Randomizer.GenerateBool();
                    var discountAmount = Randomizer.GenerateDecimal(0, (int)detail.Price);
                    var discountRate = Randomizer.GenerateDecimal(0, 100);
                    var detailQuantity = Randomizer.GenerateInt(1, QTY_LIMIT);

                    detail.Qty = detailQuantity;
                    detail.Amount = Math.Max(0, (detail.Price * detailQuantity) - (isRatePercent ? discountRate : discountAmount));
                }

                newEntity.SubTotal = newEntity.InvoiceDetail.Sum(e => e.Amount);
                newEntity.TotalAmount = newEntity.SubTotal;

                data.Add(newEntity);
            }

            Console.WriteLine($"Adding to context");
            context.Invoice.AddRange(data);
        }
    }
}
