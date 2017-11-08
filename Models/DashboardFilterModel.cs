using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using TraderData.Models.TradeImportModels;

namespace Trader.Models
{
    public class DashboardFilterModel
    {
        [Display(Name = "Instrument")]
        public int? instrumentId { get; set; }
        public SelectList InstrumentList { get; set; }

        [Display(Name = "Month")]
		public string monthNo { get; set; }
		public SelectList Month { get; set; }

        [Display(Name = "Exchange")]
		public int? exchangeId { get; set; }
		public SelectList ExchangeList { get; set; }
    }
}
