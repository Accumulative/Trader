namespace TraderData.Models.TradeImportModels
{
    public class ActiveHoldingsModel
    {

        public int TradeImportId { get; set; }
		public Instrument Instrument { get; set; }


		public int InstrumentId { get; set; }


		public decimal Value { get; set; }


		public decimal Quantity { get; set; }

        public decimal Percentage { get; set; }


    }
}
