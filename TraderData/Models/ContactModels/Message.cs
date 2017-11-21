using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TraderData.Models.ContactModels
{
    public class Message
    {
        public int MessageID { get; set; }
        [ForeignKey("Enquiry")]
        public int EnquiryID { get; set; }
        public virtual Enquiry Enquiry { get; set; }
        [Required]
        [Display(Name = "Message")]
        public string Description { get; set; }
        [Display(Name = "Timestamp")]
        public DateTime Ts { get; set; }
        // true is yes, false is no
        public bool FromCustomer { get; set; }

        public Message()
        {
            Ts = DateTime.Now;
        }
    }
}