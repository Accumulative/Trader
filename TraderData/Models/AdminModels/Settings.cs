using System;
using System.ComponentModel.DataAnnotations;

namespace TraderData.Models.AdminModels
{
    public class Settings
    {
        [Display(Name = "Price refresh time (s)")]
        public int RefreshTime { get; set; }
    }
}
