using System;
using System.ComponentModel.DataAnnotations;

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
        [Display(Name = "Current investment")]
        public decimal TotalInvested { get
            {
                return TotalBuyAmount - TotalSellAmount;
            } }
        public decimal TotalSpent {get{
                return TotalInvested + TotalFeeAmount;
            }}
        public decimal Return {get{
                return  TotalInvested / TotalSpent - 1;
            }}


        public DashboardViewModel()
        {
            
        }
    }
}
