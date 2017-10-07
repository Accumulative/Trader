using System;
using Trader.Models.TaxModels;
using Trader.Models.TradeImportModels;
using System.Collections.Generic;
using System.Linq;
using Trader.Data;
using System.Threading.Tasks;

namespace Trader.Services
{
    
    public class TaxService
    {
        private readonly ApplicationDbContext _context;
        public TaxService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaxEventModel>> CalculateTaxEvents(IEnumerable<TradeImport> trades)
        {
            var sorted = trades.OrderBy(c => c.TransactionDate);

            var instruments = await _context.InstrumentCache();
            List<TradeImport> holder = new List<TradeImport>();

            List<TaxEventModel> taxEvents = new List<TaxEventModel>();

            int id = 1;
            foreach (var item in sorted)
            {

                if (item.type == TransactionType.Buy)
                {
                    holder.Append(item);
                }
                else
                {
                    decimal remaining = item.Quantity;

                    foreach (var item2 in holder)
                    {
                        if (remaining > 0)
                        {
                            if(item2.Quantity > remaining)
                            {
								taxEvents.Append(new TaxEventModel()
								{
									TaxEventID = id,
									Quantity = remaining,
									StartValue = item2.Value,
                                    EndValue = item.Value,
									StartDate = item2.TransactionDate,
									EndDate = item.TransactionDate,
									TaxableValue = 0
								});
                                item2.Quantity -= remaining;
                                remaining = 0;
                            }
                            else
                            {
								taxEvents.Append(new TaxEventModel()
								{
									TaxEventID = id,
									Quantity = remaining - item2.Quantity,
									StartValue = item2.Value,
                                    EndValue = item.Value
									StartDate = item2.TransactionDate,
									EndDate = item.TransactionDate,
									TaxableValue = 0
								});
                                remaining -= item2.Quantity;
                                holder.Remove(item2);
                            }

                            id++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    
                }
            }

            return taxEvents;
        }
    }
}
