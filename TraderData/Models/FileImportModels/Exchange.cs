using System;
using System.ComponentModel.DataAnnotations;

namespace TraderData.Models.FileImportModels
{
    public class Exchange
    {
        [Required]
        public int ExchangeId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }
        public string URL { get; set; }


        public Exchange()
        {
        }
    }
}
