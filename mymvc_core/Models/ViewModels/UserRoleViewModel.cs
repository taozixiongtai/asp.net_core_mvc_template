using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mymvc_core.Models.ViewModels
{
    public class UserRoleViewModel
    {
        public string UserId { get; set; }

        public string RoleId { get; set; }

        public List<IdentityUser> Users { get; set; }

        public List<IdentityRole> Roles { get; set; }

        public UserRoleViewModel()
        {
            Users = new List<IdentityUser>();
            Roles = new List<IdentityRole>();
        }
    }
}
