using System;
using System.ComponentModel.DataAnnotations;

namespace Trader.Models.TradeImportModels
{
    public class TradeImportIndexModel
    {
		[Required]
		[Display(Name = "Trade Import ID")]
		public int TradeImportID { get; set; }

		[Required]
		[Display(Name = "External Reference")]
		public string ExternalReference { get; set; }

		[Required]
		public string Instrument { get; set; }

		[Required]
		public decimal Value { get; set; }

		[Required]
		public decimal Quantity { get; set; }

		[Required]
		[Display(Name = "Transaction date")]
		public DateTime TransactionDate { get; set; }

		[Required]
		[Display(Name = "Import date")]
		public DateTime ImportDate { get; set; }

		public TradeImportIndexModel()
        {
        }
    }
}
