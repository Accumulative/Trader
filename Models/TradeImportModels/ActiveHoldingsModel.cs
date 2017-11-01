﻿using System;
using TraderData.Models.TradeImportModels;

namespace TraderData.Models.TradeImportModels
{
    public class ActiveHoldingsModel
    {


		public Instrument Instrument { get; set; }


		public int InstrumentId { get; set; }


		public decimal Value { get; set; }


		public decimal Quantity { get; set; }

        public decimal Percentage { get; set; }


    }
}
