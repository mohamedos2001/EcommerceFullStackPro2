using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Dto
{
    public class RegisterUser
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string ConfirmePass { get; set; }
        public string Email { get; set; }


    }
}
