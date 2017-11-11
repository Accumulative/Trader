using System;
using System.ComponentModel.DataAnnotations;

namespace TraderData.Models.NewsModels
{
    public class NewsItem
    {
        public int NewsItemID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [Display(Name ="Timestamp")]
        public DateTime Ts { get; set; }

    }
}