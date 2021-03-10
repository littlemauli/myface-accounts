
using Microsoft.AspNetCore.Mvc;
using MyFace.Models.Request;
using MyFace.Models.Response;
using MyFace.Repositories;

namespace MyFace.Controllers
{
     [ApiController]
    [Route("/login")]
    public class LoginController :ControllerBase
    {
         private readonly IPostsRepo _posts;
        private readonly IUsersRepo _users;

        public LoginController(IPostsRepo posts, IUsersRepo users)
        {
            _posts = posts;
            _users = users;
        }
      //we want to add endpoint   which will read the authorasation header and return a response
      // who says if the data in the authorasation header is correct or not

[HttpGet("")]
[ServiceFilter(typeof(AuthFilter))]
      public ActionResult index(){
          return Ok();
      }
    }
}




