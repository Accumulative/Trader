using System;
using System.ComponentModel.DataAnnotations;

namespace Trader.Models.FileImportModels
{
    public class FileImport
    {
        public int FileImportId { get; set; }
        [Required]
        public string Filename { get; set; }
        [Required]
        public DateTime ImportDate { get; set; }

        public Exchange Exchange { get; set; }

        [Required]
        public int ExchangeId { get; set; }
    }
}