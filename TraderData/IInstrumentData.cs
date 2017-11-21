using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using TraderData.Models.InstrumentModels;
using TraderData.Models.TradeImportModels;

namespace TraderData
{
    public interface IInstrumentData
    {
        //InstrumentDataService(IMemoryCache cache, IReference reference);
        Task<decimal> GetCurrentValue(int instrumentID);
        decimal GetValueAtDate(Instrument instrument, DateTime date);
        Task<List<InstrumentData>> InstrumentPriceCache();
        Task<decimal> GetCoinbasePrice(Instrument instrument);
        Task StorePrice(int exchangeID, decimal price, int instrumentID);
        Task<List<InstrumentData>> GetValueBetweenDates(int instrumentID, DateTime fromDate, DateTime toDate);
        Task<List<InstrumentPrice>> GetStoredPricesAsync();
        Task<InstrumentPrice> GetPriceByID(int id);
        Task AddInstrumentPrice(InstrumentPrice instrumentPrice);
        Task<bool> EditInstrumentPrice(InstrumentPrice instrumentPrice);
        Task DeleteInstrumentPrice(int id);
    }
}
