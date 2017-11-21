using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TraderData.Models.ContactModels
{
    public class Enquiry
    {
        [Key]
        public int EnquiryID { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        [Required]
        [ForeignKey("ApplicationUser")]
        public string UserID { get; set; }

        [Required]
        [Display(Name = "QueryType")]
        public QueryType QueryType { get; set; }
        [Display(Name = "Timestamp")]
        public DateTime Ts { get; set; }

        public Enquiry()
        {
            Ts = DateTime.Now;
        }
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