using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TraderData.Models.TaxModels;
using TraderData.Models.TradeImportModels;

namespace TraderData
{
    public interface ITrades
    {
        List<TradeImport> getActive();
        Task<List<TradeImport>> getAll();
        void Add(TradeImport trade);
        Task<bool> Edit(TradeImport trade);
        void Delete(int id);
        Task<List<TradeImport>> getAllByUser(string userId);
        Task<List<TaxEventModel>> getTaxableEvents(List<TradeImport> trades);
        Task<TradeImport> getById(int id);
        decimal getCurrentValue(int id);
    }
}
