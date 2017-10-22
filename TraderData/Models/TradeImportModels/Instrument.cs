using System;
using System.ComponentModel.DataAnnotations;

namespace TraderData.Models.TradeImportModels
{
    public class Instrument
    {
		public int InstrumentID { get; set; }
		public string Name { get; set; }
        public Instrument()
        {
            
        }
    }
}
