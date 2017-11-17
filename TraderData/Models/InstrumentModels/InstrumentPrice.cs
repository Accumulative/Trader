using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TraderData.Models.FileImportModels;
using TraderData.Models.TradeImportModels;

namespace TraderData.Models.InstrumentModels
{
    public class InstrumentPrice
    {
        public int InstrumentPriceID { get; set; }

        public virtual Exchange Exchange { get; set; }

        [ForeignKey("Exchange")]
        [Required]
        public int ExchangeID { get; set; }

        public virtual Instrument Instrument { get; set; }

        [ForeignKey("Instrument")]
        [Required]
        public int InstrumentID { get; set; }

        public DateTime DateTime { get; set; }

        [Required]
        public decimal Price { get; set; }

        public InstrumentPrice()
        {
            DateTime = DateTime.Now;
        }
    }
}
