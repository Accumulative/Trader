using System;
using TraderData.Models.TradeImportModels;

namespace TraderData
{
    public interface IInstrumentData
    {
        decimal GetCurrentValue(Instrument instrument);
        decimal GetValueAtDate(Instrument instrument, DateTime date);
    }
}
