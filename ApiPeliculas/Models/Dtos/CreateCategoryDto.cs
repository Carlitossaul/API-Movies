using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Models.Dtos
{
    public class CreateCategoryDto
    {
       
        [Required(ErrorMessage = "The name is required")]
        [MaxLength(50, ErrorMessage = "The name can't have more than 50 characters")]
        public string Name { get; set; }
    }
}
