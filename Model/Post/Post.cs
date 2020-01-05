using BLOG_CORE.Repository;
using BLOG_CORE.ViewModels.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLOG_CORE.Helpers;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using BLOG_CORE.Entity;

namespace BLOG_CORE.Model.Post
{
    public class Post
    {
        readonly PostRepository postRepository = new PostRepository();

        //получаем все тэги
        public List<TagVM> tagAll()
        {
            var tagsAll = new List<TagVM>();
            foreach(var tag in postRepository.TagFindAll())
            {
                var tagVM = new TagVM()
                {
                    Title = tag.Title,
                };

                tagsAll.Add(tagVM);
            }
            return tagsAll;
        }

        //записываем пост
        public string PostCreate(PostVM postVM, int user_id)
        {
            //мапим вюмодель в сущность
            var post = new Entity.Post()
            {
                Title = postVM.Title,
                Content = postVM.Content,
                User_Id = user_id,
            };

            if (postRepository.PostCreate(post) == 0)
                throw new AppException("Ошибка записи в блог");

            //получаем крайний айди поста
            var postLastId = postRepository.PostGetLast();
            //получаем все тэги
            var allTags = postRepository.TagFindAll();

            foreach (var tag in postVM.Tags)
            {
                var tag_id = allTags.FirstOrDefault(x => x.Title == tag.Title).Id;
                var post_tag = new Post_Tag()
                {
                    Post_Id = postLastId,
                    Tag_Id = tag_id
                };
                //записываем тэги по посту
                postRepository.TagCreate(post_tag);
            }

            return "Запись успешно добавлена!";
        }

        public IEnumerable<PostVM> postAll()
        {
            var postAll = new List<PostVM>();
            foreach(var post in postRepository.PostFindAll())
            {
                var tagsVM = new List<TagVM>();

                //получаем список пост_блог
                var tags = postRepository.PostTagFindByID(post);

                foreach(var tag in tags)
                {
                    var tagVM = new TagVM()
                    {
                        Title = postRepository.TagFindById(tag.Tag_Id).Title
                    };
                    tagsVM.Add(tagVM);
                }

                var postVM = new PostVM()
                {
                    Title = post.Title,
                    Content = post.Content,
                    Tags = tagsVM
                };
                postAll.Add(postVM);
            }
            return postAll;
           
        }
    }
}
