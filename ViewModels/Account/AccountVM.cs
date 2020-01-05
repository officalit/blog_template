using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLOG_CORE.ViewModels.Account
{
    public class AccountVM
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public List<AccountRoleVM> Roles { get; set; }
    }
}
