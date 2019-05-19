using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace mymvc_core.Models.ViewModels
{
    public class ForgotPasswordViewModel
    {

        [Required]
        public string Email { get; set; }    //这个要改。
    }
}
