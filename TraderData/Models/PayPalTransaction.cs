using System;
using System.ComponentModel.DataAnnotations;

namespace TraderData.Models
{
    /// <summary>
    /// Create a Table in your database with these fields to store all transactions with PayPal
    /// </summary>
    public class PayPalTransaction
    {
        [Key]
        public Guid TransactionId { get; set; }
        [StringLength(128)]
        public string SaleID { get; set; }
        public string RequestId { get; set; }
        public string TrackingReference { get; set; }
        public DateTime RequestTime { get; set; }
        public string RequestStatus { get; set; }
        public string TimeStamp { get; set; }
        public string RequestError { get; set; }
        public string Token { get; set; }
        public string PayerId { get; set; }
        public string RequestData { get; set; }
        public string PaymentTransactionId { get; set; }
        public string PaymentError { get; set; }
    }
}