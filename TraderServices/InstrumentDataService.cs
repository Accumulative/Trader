using System;
using TraderData;
using TraderData.Models.TradeImportModels;

namespace TraderServices
{
    class InstrumentDataService : IInstrumentData
    {
        public decimal GetCurrentValue(Instrument instrument)
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
    }
}
