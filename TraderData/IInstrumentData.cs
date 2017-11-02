using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TraderData.Models.TradeImportModels;

namespace TraderData
{
    public interface IInstrumentData
    {
        decimal GetCurrentValue(Instrument instrument);
        decimal GetValueAtDate(Instrument instrument, DateTime date);
    }
}
