using System.ComponentModel.DataAnnotations;
using MyFace.Models.Database;
using System.Web;

namespace MyFace.Models.Request
{
    public class CreateInteractionRequest
    {
        [Required]
        public InteractionType InteractionType { get; set; }
        
        [Required]
        public int UserId { get; set; }
        
        [Required]
        public int PostId { get; set; }
    }
}