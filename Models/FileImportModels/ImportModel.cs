using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Trader.Models.TradeImportModels;

namespace Trader.Models.FileImportModels
{
    public class ImportModel
    {
        
        public SelectList ExchangeList { get; set; }

        public SelectList InstrumentList { get; set; }
        [Required]
        [Display(Name="Exchange:")]
        public int selExchange { get; set; }
        [Required]
        [Display(Name = "Instrument:")]
        public int selInstrument { get; set; }
    }
}
