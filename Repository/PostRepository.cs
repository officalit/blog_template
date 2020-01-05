using BLOG_CORE.Entity;
using BLOG_CORE.ViewModels.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLOG_CORE.Repository
{
    public class PostRepository : BaseRepository
    {
        public IEnumerable<Post> PostFindAll()
        {
            return Query<Post>("SELECT * FROM POSTS");
        }

        public List<Tag> TagFindAll()
        {
            return Query<Tag>("SELECT * FROM TAGS");
        }

        public Tag TagFindById(int id)
        {
            return QueryFirstOrDefault<Tag>("SELECT * FROM TAGS WHERE ID = :ID", new { ID = id });
        }


        public IEnumerable<Post_Tag> PostTagFindByID(Post post)
        {
            return Query<Post_Tag>("SELECT * FROM POSTS_TAGS WHERE POST_ID = :ID", post);
        }

        public int PostCreate(Post post)
        {
            return Execute("INSERT INTO POSTS (TITLE,CONTENT,USER_ID) VALUES (:TITLE,:CONTENT,:USER_ID)", post);
        }

        public int PostGetLast()
        {
            return QueryFirstOrDefault<int>("SELECT ROWID FROM POSTS ORDER BY ROWID DESC LIMIT 1");
        }

        public int TagCreate(Post_Tag post_tag)
        {
            return Execute("INSERT INTO POSTS_TAGS (POST_ID,TAG_ID) VALUES (:POST_ID,:TAG_ID)", post_tag);
        }


    }
}
