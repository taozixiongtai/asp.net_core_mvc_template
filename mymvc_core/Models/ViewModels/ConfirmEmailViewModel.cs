using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace mymvc_core.Models.ViewModels
{
    public class ConfirmEmailViewModel
    {
        [Required]
        public string userName { get; set; }

        [Required]
        public string emailAdress { get; set; }
    }
}
