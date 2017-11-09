﻿using System.Collections.Generic;
using TraderData.Models.TradeImportModels;

namespace Trader.Models
{
    public class InstrumentInfoViewModel
    {
        public IEnumerable<InstrumentInfoModel> instrumentInfoList { get; set; }
    }
    public class InstrumentInfoModel
    {
        public Instrument instrument { get; set; }
        public decimal value { get; set; }
    }
}
