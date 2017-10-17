using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Trader.Models.TradeImportModels
{
    public class TradeImportViewModel
    {
		[Required]
		[Display(Name = "External Reference")]
		public string ExternalReference { get; set; }

		[Required]
		public SelectList InstrumentList { get; set; }

		[Required]
		public int InstrumentId { get; set; }

		[Required]
		public decimal Value { get; set; }

		[Required]
		public decimal Quantity { get; set; }

		[Required]
		[Display(Name = "Transaction date")]
		public DateTime TransactionDate { get; set; }

        public TradeImportViewModel()
        {
        }
    }
}
