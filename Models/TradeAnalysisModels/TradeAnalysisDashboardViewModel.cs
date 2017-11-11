using System;
using System.Collections.Generic;
using TraderData.Models.TradeImportModels;
using TraderData.Models.TaxModels;

namespace Trader.Models.TradeAnalysisModels
{
    public class TradeAnalysisDashboardViewModel
    {
        public List<TradeImport> pastTrades { get; set; }
        public List<ActiveHoldingsModel> activeTrades { get; set; }
        public List<TaxEventModel> bestTrades { get; set; }
        public List<TaxEventModel> worstTrades { get; set; }
    }
}
