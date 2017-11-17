using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TraderData.Models.FileImportModels;

namespace TraderData.Models.TradeImportModels
{
    public class TradeImport
	{
		[Required]
		[Display(Name = "Trade Import ID")]
		public int TradeImportID { get; set; }

        [Required]
        [Display(Name = "External Reference")]
        public string ExternalReference { get; set; }

        public int FileImportId { get; set; }

        public FileImport FileImport { get; set; }

		[Required]
		[Display(Name = "Transaction type")]
        public TransactionType TransactionType { get; set; }


		public Instrument Instrument { get; set; }

        [ForeignKey("Instrument")]
        [Required]
        public int InstrumentId { get; set; }

        [Required]
		public decimal Value { get; set; }

        [Required]
        public decimal Quantity { get; set; }

        [Required]
        [Display(Name = "Transaction date")]
        public DateTime TransactionDate { get; set; }

        [Display(Name = "Import date")]
        public DateTime ImportDate { get; set; } 

        public decimal TransactionFee { get; set; }


        public Currency Currency { get; set; }

        [Required]
        public string UserID { get; set; }

        [ForeignKey("UserID")]
        public virtual ApplicationUser User { get; set; }

        public TradeImport()
        {
            ImportDate = DateTime.Now;
        }
	}
}
