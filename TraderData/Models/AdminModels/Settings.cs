using System;
using System.ComponentModel.DataAnnotations;

namespace TraderData.Models.AdminModels
{
    public class Settings
    {
        public int SettingsID { get; set; }

        [Display(Name = "Price refresh time (s)")]
        public int RefreshTime { get; set; }
    }
}
