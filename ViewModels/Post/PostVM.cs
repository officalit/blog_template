using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLOG_CORE.ViewModels.Post
{
    public class PostVM
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public List<TagVM> Tags {get;set;}
    }
}
