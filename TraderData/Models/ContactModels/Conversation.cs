using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TraderData.Models
{
    public class Enquiry
    {
        [Key]
        public int EnquiryID { get; set; }
        [StringLength(128)]
        public string CustomerId { get; set; }
        [Required]
        [Display(Name = "QueryType")]
        public QueryType QueryType { get; set; }
        [Display(Name = "Timestamp")]
        public DateTime Ts { get; set; }
    }
    public enum QueryType
    {
        [Display(Name = "ProductQuery")]
        ProductQuery = 1,
        [Display(Name = "OrderQuery")]
        OrderQuery = 2,
        [Display(Name = "GeneralQuery")]
        GeneralQuery = 3
    }
}