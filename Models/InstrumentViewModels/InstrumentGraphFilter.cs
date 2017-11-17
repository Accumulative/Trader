using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Trader.Models.InstrumentViewModels
{
    public class InstrumentGraphFilter
    {
        [Display(Name = "Instrument")]
        public int? instrumentId { get; set; }
        public SelectList InstrumentList { get; set; }

        [Display(Name = "Month")]
        public string monthNo { get; set; }
        public SelectList Month { get; set; }
    }
}
