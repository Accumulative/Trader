using System;
using System.ComponentModel.DataAnnotations;

namespace Trader.Models.ReportModels
{
    public class DataSelectModel
    {
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }

        public DataSelectModel()
        {
        }
    }
}
