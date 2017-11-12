using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TraderData.Models;

namespace Trader.Models.AccountViewModels
{
    public class UserDetailsViewModel
    {
        public ApplicationUser user { get; set; }
        public List<string> roles { get; set; }
    }
}
