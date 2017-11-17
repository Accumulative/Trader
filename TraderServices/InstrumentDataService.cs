using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using TraderData;
using TraderData.Models.InstrumentModels;
using TraderData.Models.TradeImportModels;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

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
        private readonly ApplicationDbContext _context;

        private const string BlahCacheKey = "blah-cache-key3";
        public InstrumentDataService(IMemoryCache cache, IReference reference, ApplicationDbContext context)
        {
            _reference = reference;
            _cache = cache;
            _context = context;
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
            var results = insList.Select(x => new InstrumentData
            {
                instrument = x
            }).ToList();

            foreach(var ins in results)
            {
                ins.price = await GetCoinbasePrice(ins.instrument);
            }

            instrumentCache = new InstrumentDataCacheStore{ 
                instruments = results.ToList(),
                lastStored = DateTime.Now
            };

			_cache.Set(BlahCacheKey, instrumentCache);

			return instrumentCache.instruments;
			/*return await Instrument.ToListAsync();*/
		}
        public async Task<decimal> GetCoinbasePrice(Instrument instrument)
        {
            var url = "/v2/prices/" + instrument.Ticker + "/buy";
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri("https://api.coinbase.com");
                    var response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    var stringResult = await response.Content.ReadAsStringAsync();
                    var rawWeather = JsonConvert.DeserializeObject<CoinbasePrice>(stringResult);
                    var price = decimal.Parse(rawWeather.data.amount);

                    // Store the price
                    var exchange = await _reference.GetExchangeByNameAsync("Coinbase");
                    await StorePrice(exchange.ExchangeId, price, instrument.InstrumentID );

                    // return the price
                    return price;
                }
                catch (HttpRequestException httpRequestException)
                {
                    return 0;
                }
            }

		}

        public async Task StorePrice (int exchangeID, decimal price, int instrumentID)
        {
            _context.Add(new InstrumentPrice
            {
                ExchangeID = exchangeID,
                Price = price,
                InstrumentID = instrumentID,
                DateTime = DateTime.Now
            });
            await _context.SaveChangesAsync();
        }
        public async Task<List<InstrumentData>> GetValueBetweenDates(int instrumentID, DateTime fromDate, DateTime toDate)
        {
            var exchange = await _reference.GetExchangeByNameAsync("Coinbase");
            var ins = await _context.InstrumentPrice.Include(x => x.Instrument).Where(x => x.DateTime <= toDate && x.DateTime >= fromDate && x.ExchangeID == exchange.ExchangeId && x.Instrument.InstrumentID == instrumentID)
                .Select(x => new InstrumentData
                {
                    instrument = x.Instrument,
                    price = x.Price,
                    dateTime = x.DateTime
                }).ToListAsync();
            return ins;
        }
    }
    class InstrumentDataCacheStore
    {
        public List<InstrumentData> instruments { get; set; }
        public DateTime lastStored { get; set; }
    }

}
