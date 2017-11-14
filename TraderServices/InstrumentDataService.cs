using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using TraderData;
using TraderData.Models.InstrumentModels;
using TraderData.Models.TradeImportModels;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace TraderServices
{
    public class CoinbasePrice
    {
        public Data data { get; set; }
    }
    public class Data
    {
        public string currency { get; set; }
        public string amount { get; set; }

    }
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
            var tasks = insList.Select(async x => new InstrumentData
            {
                instrument = x,
                price = await GetCoinbasePrice(x.Ticker)
            }).ToList();

            var results = await Task.WhenAll(tasks);

            instrumentCache = new InstrumentDataCacheStore{ 
                instruments = results.ToList(),
                lastStored = DateTime.Now
            };

			_cache.Set(BlahCacheKey, instrumentCache);

			return instrumentCache.instruments;
			/*return await Instrument.ToListAsync();*/
		}
        public async Task<decimal> GetCoinbasePrice(string pair)
        {
            var url = "/v2/prices/" + pair + "/buy";
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri("https://api.coinbase.com");
                    var response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    var stringResult = await response.Content.ReadAsStringAsync();
                    var rawWeather = JsonConvert.DeserializeObject<CoinbasePrice>(stringResult);
                    return decimal.Parse(rawWeather.data.amount);
                }
                catch (HttpRequestException httpRequestException)
                {
                    return 0;
                }
            }

		}
    }
    class InstrumentDataCacheStore
    {
        public List<InstrumentData> instruments { get; set; }
        public DateTime lastStored { get; set; }
    }

}
