using System.ComponentModel.DataAnnotations;

namespace TraderData.Models
{
    public class CultureCountry
    {
        public string CultureCountryID { get; set; }
        public Country country { get; set; }
        public string culture { get; set; }
    }
    public enum Country
    {
        [Display(Name = "UK")]
        UK = 1,
        [Display(Name = "Japan")]
        JP = 2
    }
}