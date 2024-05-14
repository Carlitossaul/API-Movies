using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPeliculas.Models
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string UrlImage { get; set; }
        public int Duration { get; set; }
        public string Description { get; set; }
        public enum TypeClassification { seven, thirteen, sixteen, eightteen }
        public TypeClassification Classification { get; set; }
        public DateTime CreatedDate { get; set; }
        [ForeignKey("categoryId")]
        public int categoryId { get; set; }
        public Category Category { get; set; }

    }
}
