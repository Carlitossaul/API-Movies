using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Models.Dtos
{
    public class CategoryDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The name is required")]
        [MaxLength(50, ErrorMessage = "The name can't have more than 50 characters")]
        public string Name { get; set; }
    }
}
