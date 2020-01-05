using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLOG_CORE.Entity
{
    public class User_Role
    {
        public int Id { get; set; }
        public int User_Id { get; set; }
        public int Role_Id { get; set; }
    }
}
