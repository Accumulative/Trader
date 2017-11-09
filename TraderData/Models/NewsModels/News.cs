using System;
using System.ComponentModel.DataAnnotations;

namespace TraderData.Models
{
    public class News
    {
        public int NewsID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [Display(Name ="Timestamp")]
        public DateTime Ts { get; set; }

    }
}