using System;
using TraderData.Models.TradeImportModels;

namespace Trader.Models.TradeAnalysisModels
{
    public class TradeAnalysisModel
    {
        public TradeImport startTrade { get; set; }
        public TradeImport endTrade { get; set; }

        public decimal MoneyTaken { get {
				if (!endTrade.Equals(null))
					return ((TradeImport)endTrade).Value * endTrade.Quantity - TotalFee;
				else
					return CurrentValue * startTrade.Quantity - TotalFee * 2;
            } }
		public decimal CurrentMoneyTaken
		{
			get
			{
				return CurrentValue * startTrade.Quantity - TotalFee * 2;
			}
		}
        public decimal CurrentValue { get; set; }
		public decimal CurrentProfit
		{
			get
			{
                return  CurrentMoneyTaken - (startTrade.Value * startTrade.Quantity);
			}
		}

        public string Return {
            get {
                return String.Format("{0:P2}", Profit / (startTrade.Value * startTrade.Quantity));
            }
        }
		public string CurrentReturn
		{
			get
			{
                return String.Format("{0:P2}", CurrentProfit / (startTrade.Value * startTrade.Quantity));
			}
		}

        public decimal Profit { get {
                if (!endTrade.Equals(null))
                    return MoneyTaken - (startTrade.Value * startTrade.Quantity);
                else
                    return CurrentMoneyTaken - (startTrade.Value * startTrade.Quantity);
            }}
		public decimal TotalFee
		{
			get
			{
                if (!endTrade.Equals(null))
                    return ((TradeImport)endTrade).TransactionFee + startTrade.TransactionFee;
                else
                    return startTrade.TransactionFee * 2;
				
			}
		}

        public TradeAnalysisModel()
        {
        }
    }
}
