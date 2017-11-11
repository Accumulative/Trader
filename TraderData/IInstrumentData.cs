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
    }
}
