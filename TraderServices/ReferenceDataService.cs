using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TraderData;
using TraderData.Models;
using TraderData.Models.FileImportModels;
using TraderData.Models.TradeImportModels;
using TraderData.Models.AdminModels;

namespace TraderServices
{
    public class ReferenceDataService : IReference
    {
		private readonly ApplicationDbContext _context;

		public ReferenceDataService(ApplicationDbContext context)
		{
			_context = context;
		}

        public async Task AddExchange(Exchange exchange)
        {
            _context.Add(exchange);
            await _context.SaveChangesAsync();
            await _context.UpdateExchangeCache();
        }

		public async Task<Settings> GetSettings()
		{
            var sett = await _context.SettingsCache();
            return sett;
		}

        public async Task AddInstrument(Instrument instrument)
        {
            _context.Add(instrument);
            await _context.SaveChangesAsync();
            await _context.UpdateInstrumentCache();
        }

        public async Task DeleteExchange(int id)
        {
            var exchange = await GetExchangeById(id);
            _context.Exchange.Remove(exchange);
            await _context.SaveChangesAsync();
            await _context.UpdateExchangeCache();
        }

        public async Task DeleteInstrument(int id)
        {
            var instrument = await GetInstrumentById(id);
            _context.Instrument.Remove(instrument);
            await _context.SaveChangesAsync();
            await _context.UpdateInstrumentCache();
        }

        public async Task<bool> EditExchange(Exchange exchange)
        {
            try
            {
                _context.Update(exchange);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
            await _context.UpdateExchangeCache();
            return true;
        }

        public async Task<bool> EditInstrument(Instrument instrument)
        {
            try
            {
                _context.Update(instrument);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
            await _context.UpdateInstrumentCache();
            return true;
        }

        public IEnumerable<Currency> getCurrencies()
        {
            return Enum.GetValues(typeof(Currency)).Cast<Currency>();
        }

        public async Task<Exchange> GetExchangeById(int id)
        {
            return await _context.Exchange
                .SingleOrDefaultAsync(m => m.ExchangeId == id);
        }

        public async Task<IEnumerable<Exchange>> getExchanges()
        {
			return await _context.ExchangeCache();
        }

        public async Task<IEnumerable<Instrument>> getInstruments()
        {
            return await _context.InstrumentCache();
        }

        public bool ExchangeExists(int id)
        {
            return _context.Exchange.Any(e => e.ExchangeId == id);
        }

        public bool InstrumentExists(int id)
        {
            return _context.Instrument.Any(e => e.InstrumentID == id);
        }

        public async Task<Instrument> GetInstrumentById(int id)
        {
            return await _context.Instrument
                .SingleOrDefaultAsync(m => m.InstrumentID == id);
        }
        
    }
}
