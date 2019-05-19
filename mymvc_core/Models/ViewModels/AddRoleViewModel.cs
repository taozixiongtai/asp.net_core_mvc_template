using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace mymvc_core.Models.ViewModels
{
    public class AddRoleViewModel
    {
        [Remote("CheckRoleExist", "Identity", ErrorMessage = "角色已存在")]
        [Display(Name = "添加的角色名")]
        [Required]
        public string roleName { get; set; }


        public List<string> Users { get; set; }
    }
}
