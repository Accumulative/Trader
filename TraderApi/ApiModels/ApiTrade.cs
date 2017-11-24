using System;
namespace TraderApi.ApiModels
{
    public class ApiTrade
    {
        public string Name { get; set; }
        public decimal Quantity { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal Value { get; set; }
        public decimal Total { get { return Value * Quantity; }}
        public decimal Return { get { return Value / OriginalPrice - 1; } }
    }
}
