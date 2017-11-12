using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Trader.Models.AccountViewModels
{
    public class EditUserViewModel
    {
        [Required]
        public SelectList RoleList { get; set; }

        public string selRole { get; set; }

        public string UserID { get; set; }
    }
}
