using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TraderData.Models.FileImportModels
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

        [Required]
		public string UserID { get; set; }

		[ForeignKey("UserID")]
		public virtual ApplicationUser User { get; set; }
    }
}