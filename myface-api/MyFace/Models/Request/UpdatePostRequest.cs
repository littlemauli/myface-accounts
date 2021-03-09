using System;
using System.Web;

namespace MyFace.Models.Request
{
    public class UpdatePostRequest
    {
        public string Message { get; set; }
        public string ImageUrl { get; set; }
    }
}