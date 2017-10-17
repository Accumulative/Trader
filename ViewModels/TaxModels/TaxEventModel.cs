using System;
using Trader.Models.TradeImportModels;

namespace Trader.Models.TaxModels
{
    public class TaxEventModel
    {
        
        public int TaxEventID { get; set; }
        public decimal StartValue { get; set; }
        public decimal EndValue { get; set; }
        public decimal Quantity { get; set; }
        public Instrument Instrument { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalValue { get { return EndValue * Quantity; } }
        public decimal ProfitLoss { get { return (EndValue - StartValue) * Quantity - Fee; } }
        public decimal Fee { get; set; }
        public decimal TaxableValue { get; set; }

        public TaxEventModel()
        {
            
        }

    }
}
