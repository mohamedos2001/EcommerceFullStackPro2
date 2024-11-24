using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Dto
{
    public class LoginUser
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
