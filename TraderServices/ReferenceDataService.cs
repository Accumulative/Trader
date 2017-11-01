using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TraderData;
using TraderData.Models;
using TraderData.Models.FileImportModels;
using TraderData.Models.TradeImportModels;

namespace TraderServices
{
    public class ReferenceDataService : IReference
    {
		private readonly ApplicationDbContext _context;

		public ReferenceDataService(ApplicationDbContext context)
		{
			_context = context;
		}

        public void AddExchange(Exchange exchange)
        {
            throw new NotImplementedException();
        }

        public void AddInstrument(Instrument instrument)
        {
            throw new NotImplementedException();
        }

        public void DeleteExchange(int id)
        {
            throw new NotImplementedException();
        }

        public void DeleteInstrument(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> EditExchange(Exchange instrument)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EditInstrument(Instrument instrument)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Currency> getCurrencies()
        {
            return Enum.GetValues(typeof(Currency)).Cast<Currency>();
        }

        public async Task<IEnumerable<Exchange>> getExchanges()
        {
			return await _context.ExchangeCache();
        }

        public async Task<IEnumerable<Instrument>> getInstruments()
        {
            return await _context.InstrumentCache();
        }
    }
}
