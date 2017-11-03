using System.Collections.Generic;
using System.Threading.Tasks;
using TraderData.Models;
using TraderData.Models.FileImportModels;
using TraderData.Models.TradeImportModels;

namespace TraderData
{
    public interface IReference
    {
		Task<IEnumerable<Instrument>> getInstruments();
		void AddInstrument(Instrument instrument);
		Task<bool> EditInstrument(Instrument instrument);
		void DeleteInstrument(int id);
		Task<IEnumerable<Exchange>> getExchanges();
		void AddExchange(Exchange exchange);
		Task<bool> EditExchange(Exchange instrument);
		void DeleteExchange(int id);
        IEnumerable<Currency> getCurrencies();
    }
}
