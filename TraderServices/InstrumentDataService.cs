using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using TraderData;
using TraderData.Models.InstrumentModels;
using TraderData.Models.TradeImportModels;

namespace TraderServices
{
    public class InstrumentDataService : IInstrumentData
    {
        private readonly IMemoryCache _cache;
        private readonly IReference _reference;
        private const string BlahCacheKey = "blah-cache-key3";
        public InstrumentDataService(IMemoryCache cache, IReference reference)
        {
            _reference = reference;
            _cache = cache;
        }
        public async Task<decimal> GetCurrentValue(int instrumentID)
        {
            var instrumentData = await InstrumentPriceCache();
            return instrumentData.FirstOrDefault(x => x.instrument.InstrumentID == instrumentID).price;
        }
        public decimal GetValueAtDate(Instrument instrument, DateTime date)
        {
            Random RandGen = new Random();
            if (instrument.Name == "Ethereum")
            {
                return (decimal)RandGen.Next(200, 300);
            }
            else
            {
                return (decimal)RandGen.Next(20, 40);
            }
        }
		public async Task<List<InstrumentData>> InstrumentPriceCache()
		{
            Random RandGen = new Random();
			if (_cache.TryGetValue(BlahCacheKey, out InstrumentDataCacheStore instrumentCache))
			{
                var settings = await _reference.GetSettings();
                if (instrumentCache.lastStored.AddSeconds(settings.RefreshTime)> DateTime.Now)
                {
                    return instrumentCache.instruments;
                }

			}

            var insList = await _reference.getInstruments();
            instrumentCache = new InstrumentDataCacheStore{ 
                instruments = insList.Select(x => new InstrumentData
                {
                    instrument = x,
                    price = (decimal)RandGen.Next(20, 40)
                }).ToList(),
                lastStored = DateTime.Now
            };

			_cache.Set(BlahCacheKey, instrumentCache);

			return instrumentCache.instruments;
			/*return await Instrument.ToListAsync();*/
		}
    }
    class InstrumentDataCacheStore
    {
        public List<InstrumentData> instruments { get; set; }
        public DateTime lastStored { get; set; }
    }

}
