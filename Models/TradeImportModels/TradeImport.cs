using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Trader.Models.FileImportModels;

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
        public int FileImportId { get; set; }

        public FileImport FileImport { get; set; }

		[Required]
		[Display(Name = "Transaction type")]
        public TransactionType TransactionType { get; set; }

        [Required]
		public Instrument Instrument { get; set; }

        [ForeignKey("Instrument")]
        public int InstrumentId { get; set; }

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

        public decimal TransactionFee { get; set; }

        public Currency Currency { get; set; }

	}
}
