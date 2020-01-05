using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLOG_CORE.Entity
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int User_Id { get; set; }
    }
}
