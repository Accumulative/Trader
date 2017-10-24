using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TraderData;
using TraderData.Models.TaxModels;
using TraderData.Models.TradeImportModels;

namespace TraderServices
{
    public class TradeImportService : ITrades
    {

        private readonly ApplicationDbContext _context;

        public TradeImportService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async void Add(TradeImport trade)
        {
			_context.Add(trade);
			await _context.SaveChangesAsync();
        }

        public async void Delete(int id)
        {
			var tradeImport = await _context.TradeImport.SingleOrDefaultAsync(m => m.TradeImportID == id);
			_context.TradeImport.Remove(tradeImport);
			await _context.SaveChangesAsync();
        }

        public async Task<bool> Edit(TradeImport trade)
        {
			try
			{
				_context.Update(trade);
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
                return false;
			}
            return true;
        }

        public List<TradeImport> getActive()
        {
            throw new NotImplementedException();
        }

        public async Task<List<TradeImport>> getAll()
        {
            return await _context.TradeImport.ToListAsync();
        }

        public async Task<List<TradeImport>> getAllByUser(string userId)
        {
            return await _context.TradeImport.Include(s => s.Instrument).Where(x => x.UserID == userId).ToListAsync();
        }

        public async Task<TradeImport> getById(int id)
        {
            return await _context.TradeImport.FirstOrDefaultAsync(x => x.TradeImportID == id);
        }

        public decimal getCurrentValue(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<TaxEventModel>> getTaxableEvents(List<TradeImport> trades)
        {
			var sorted = trades.OrderBy(c => c.TransactionDate);

			var instruments = await _context.InstrumentCache();
			List<TradeImport> holder = new List<TradeImport>();

			List<TaxEventModel> taxEvents = new List<TaxEventModel>();

			int id = 1;
			foreach (var item in sorted)
			{

				if (item.TransactionType == TransactionType.Buy)
				{
					holder.Add(item);
				}
				else
				{
					decimal remaining = item.Quantity;

					foreach (var item2 in holder.Where(x => x.InstrumentId == item.InstrumentId).ToArray())
					//Iterate through a copy so we dont get an error about changing array mid loop (ToArray)
					{
						if (remaining > 0)
						{
							if (item2.Quantity > remaining)
							{
								taxEvents.Add(new TaxEventModel()
								{
									TaxEventID = id,
									Quantity = remaining,
									StartValue = item2.Value,
									EndValue = item.Value,
									Instrument = item.Instrument,
									StartDate = item2.TransactionDate,
									EndDate = item.TransactionDate,
									TaxableValue = 0,
									Fee = item.TransactionFee + item2.TransactionFee * remaining / item2.Quantity

								});
								item2.Quantity -= remaining;
								remaining = 0;
							}
							else
							{
								taxEvents.Add(new TaxEventModel()
								{
									TaxEventID = id,
									Quantity = item2.Quantity,
									StartValue = item2.Value,
									EndValue = item.Value,
									Instrument = item.Instrument,
									StartDate = item2.TransactionDate,
									EndDate = item.TransactionDate,
									TaxableValue = 0,
									Fee = item.TransactionFee + item2.TransactionFee * item2.Quantity / remaining
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
