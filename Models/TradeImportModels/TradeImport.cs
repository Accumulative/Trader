using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Trader.Models.TradeImportModels
{
	public class TradeImport
	{
		[Required]
		[Display(Name = "Trade Import ID")]
		public int TradeImportID { get; set; }

        [Required]
        [Display(Name = "External Reference")]
        public string ExternalReference { get; set; }

		[Required]
		public InstrumentModel Instrument { get; set; }

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
	}
}
