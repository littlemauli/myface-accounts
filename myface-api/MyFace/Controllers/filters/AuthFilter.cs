using System;
using MyFace.Repositories;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MyFace.Controllers
{
    // public class IActionFilter
    // {

    // }
    public class AuthFilter : IActionFilter
    {

        private readonly IUsersRepo _users;

        public AuthFilter(IUsersRepo users)
        {
            _users = users;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {

            string authHeader = context.HttpContext.Request.Headers["Authorization"];
            Console.WriteLine(authHeader);
            if (authHeader != null && authHeader.StartsWith("Basic"))
            {
                string encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
                Encoding encoding = Encoding.GetEncoding("iso-8859-1");
                string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));

                int seperatorIndex = usernamePassword.IndexOf(':');

                var header_username = usernamePassword.Substring(0, seperatorIndex);
                var header_password = usernamePassword.Substring(seperatorIndex + 1);

                var userInDatabase = _users.GetByUsername(header_username);

                var Header_Hashed_password = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                      password: header_password,
                      salt: Convert.FromBase64String(userInDatabase.Salt),
                      prf: KeyDerivationPrf.HMACSHA1,
                      iterationCount: 10000,
                      numBytesRequested: 256 / 8));

                if (userInDatabase.Hashed_password != Header_Hashed_password)
                {
                    Console.WriteLine("The password doesn't match the database");
                    // return Unauthorized("The password doesn't match the database");
                }
            }
            else
            {
                Console.WriteLine("bad header");
                //return Unauthorized("The authorization header is either empty or isn't Basic.");
            }


            // Do something before the action executes.
            //MyDebug.Write(MethodBase.GetCurrentMethod(), context.HttpContext.Request.Path);
        }
    }

}