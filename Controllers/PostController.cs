using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BLOG_CORE.Helpers;
using BLOG_CORE.Model.Post;
using BLOG_CORE.ViewModels.Post;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BLOG_CORE.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class PostController : Controller
    {
        readonly Post postModel = new Post();

        [Route("tags")]
        [HttpGet]
        public IActionResult Tags()
        {
            return Ok(postModel.tagAll());
        }

        [Route("create")]
        [HttpPost]
        public IActionResult CreatePost([FromBody]PostVM postVM)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(x => x.Type == "uniq_id").Value;
                return Ok(new { message = postModel.PostCreate(postVM, Convert.ToInt32(userId))});
            }
            catch (AppException ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        [Route("posts")]
        public IActionResult PostsAll()
        {
            return Ok(postModel.postAll());
        }

    }
}