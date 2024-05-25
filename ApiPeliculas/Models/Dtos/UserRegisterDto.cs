using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Models.Dtos
{
    public class UserRegisterDto
    {
        [Required(ErrorMessage = "The Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "The UserName is required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "The Password is required")]
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
