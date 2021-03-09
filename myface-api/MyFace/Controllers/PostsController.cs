using System;
using Microsoft.AspNetCore.Mvc;
using MyFace.Models.Request;
using MyFace.Models.Response;
using MyFace.Repositories;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;


namespace MyFace.Controllers
{    
    [ApiController]
    [Route("/posts")]
    public class PostsController : ControllerBase
    {    
        private readonly IPostsRepo _posts;
        private readonly IUsersRepo _users;

        public PostsController(IPostsRepo posts, IUsersRepo users)
        {
            _posts = posts;
            _users = users;
        }
        
        [HttpGet("")]
        public ActionResult<PostListResponse> Search([FromQuery] PostSearchRequest searchRequest)
        {
            var posts = _posts.Search(searchRequest);
            var postCount = _posts.Count(searchRequest);
            return PostListResponse.Create(searchRequest, posts, postCount);
        }

        [HttpGet("{id}")]
        public ActionResult<PostResponse> GetById([FromRoute] int id)
        {
            var post = _posts.GetById(id);
            return new PostResponse(post);
        }

        [HttpPost("create")]
        public IActionResult Create([FromHeader]string authorization,[FromBody] CreatePostRequest newPost)
        {    
         string authHeader = Request.Headers["Authorization"];
         Console.WriteLine(authHeader);
        if (authHeader != null && authHeader.StartsWith("Basic")) {
    string encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
    Encoding encoding = Encoding.GetEncoding("iso-8859-1");
    string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));

    int seperatorIndex = usernamePassword.IndexOf(':');

    var header_username = usernamePassword.Substring(0, seperatorIndex);
    var header_password = usernamePassword.Substring(seperatorIndex + 1);

     var userInDatabase =_users.GetByUsername(header_username);
     
      var Header_Hashed_password =Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: header_password,
            salt:  Convert.FromBase64String(userInDatabase.Salt),
            prf: KeyDerivationPrf.HMACSHA1,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));

      if( userInDatabase.Hashed_password != Header_Hashed_password) 
      {
           Console.WriteLine("The password doesn't match the database");
          return Unauthorized("The password doesn't match the database");
         
      }     

} else {
    //Handle what happens if that isn't the case
    return BadRequest("The authorization header is either empty or isn't Basic.");
}

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var post = _posts.Create(newPost);

            var url = Url.Action("GetById", new { id = post.Id });
            var postResponse = new PostResponse(post);
            return Created(url, postResponse);
        }

        [HttpPatch("{id}/update")]
        public ActionResult<PostResponse> Update([FromRoute] int id, [FromBody] UpdatePostRequest update)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var post = _posts.Update(id, update);
            return new PostResponse(post);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            _posts.Delete(id);
            return Ok();
        }
    }
}