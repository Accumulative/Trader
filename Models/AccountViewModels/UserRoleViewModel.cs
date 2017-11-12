using System.ComponentModel.DataAnnotations;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Trader.Models.AccountViewModels
{
    public class UserRoleViewModel
    {
        [Required]
        public string Name { get; set; }
    }
}
