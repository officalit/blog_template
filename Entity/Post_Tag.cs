using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLOG_CORE.Entity
{
    public class Post_Tag
    {
        public int Id { get; set; }
        public int Post_Id { get; set; }
        public int Tag_Id { get; set; }
    }
}
