using System;
using MyFace.Models.Database;
using System.Web;

namespace MyFace.Models.Response
{
    public class PostResponse
    {
        private readonly Post _post;

        public PostResponse(Post post)
        {
            _post = post;
        }
        
        public int Id => _post.Id;
        public string Message => _post.Message;
        public string ImageUrl => _post.ImageUrl;
        public DateTime PostedAt => _post.PostedAt;
        public int UserId => _post.UserId;
    }
}