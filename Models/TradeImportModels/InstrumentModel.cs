using System;
using System.ComponentModel.DataAnnotations;

namespace Trader.Models.TradeImportModels
{
    public class InstrumentModel
    {
		public int InstrumentModelID { get; set; }
		public string Name { get; set; }
        public InstrumentModel()
        {
            
        }
    }
}
