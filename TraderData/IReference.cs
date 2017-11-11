using System.Collections.Generic;
using System.Threading.Tasks;
using TraderData.Models;
using TraderData.Models.AdminModels;
using TraderData.Models.FileImportModels;
using TraderData.Models.TradeImportModels;

namespace TraderData
{
    public interface IReference
    {
		Task<IEnumerable<Instrument>> getInstruments();
        Task AddInstrument(Instrument instrument);
        Task<bool> EditInstrument(Instrument instrument);
        Task DeleteInstrument(int id);
        Task<IEnumerable<Exchange>> getExchanges();
		Task AddExchange(Exchange exchange);
        Task<bool> EditExchange(Exchange exchange);
        bool ExchangeExists(int id);
        Task DeleteExchange(int id);
        IEnumerable<Currency> getCurrencies();
        Task<Exchange> GetExchangeById(int id);
        Task<Instrument> GetInstrumentById(int id);
        bool InstrumentExists(int id);
        Task<Settings> GetSettings();
    }
}
