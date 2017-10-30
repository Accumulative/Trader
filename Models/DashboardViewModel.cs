using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TraderData.Models.TradeImportModels;

namespace Trader.Models
{
    public class DashboardViewModel
    {
        [Display(Name = "Total sells")]
        public decimal TotalSellAmount { get; set; }
		[Display(Name = "Total fees")]
		public decimal TotalFeeAmount { get; set; }
        [Display(Name = "Total buys")]
        public decimal TotalBuyAmount { get; set; }
        [Display(Name = "Current profit")]
        public decimal TotalInvested { get
            {
                return TotalSellAmount - TotalBuyAmount - TotalFeeAmount;
            } }
        public decimal TotalSpent {get{
                return TotalBuyAmount + TotalFeeAmount;
            }}
        public decimal Return {get{
                return  TotalInvested / (TotalSellAmount - TotalBuyAmount) - 1;
            }}
        public List<TradeImport> ActiveTrades { get; set; }

        public DashboardViewModel()
        {
            
        }
    }
}
