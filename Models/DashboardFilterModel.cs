using Microsoft.AspNetCore.Mvc.Rendering;
using TraderData.Models.TradeImportModels;

namespace Trader.Models
{
    public class DashboardFilterModel
    {
        public int instrumentId { get; set; }
        public SelectList InstrumentList { get; set; }
    }
}
