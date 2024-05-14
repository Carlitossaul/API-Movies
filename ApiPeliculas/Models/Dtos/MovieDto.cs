using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Models.Dtos
{
    public class MovieDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "The name is required")]
        public string Name { get; set; }
        public string UrlImage { get; set; }
        [Required(ErrorMessage = "The duration is required")]
        public int Duration { get; set; }
        [Required(ErrorMessage = "The description is required")]
        public string Description { get; set; }
        public enum TypeClassification { seven, thirteen, sixteen, eightteen }
        public TypeClassification Classification { get; set; }
        public DateTime CreatedDate { get; set; }
        public int categoryId { get; set; }
    }
}
