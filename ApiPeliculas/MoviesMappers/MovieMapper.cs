using ApiPeliculas.Models;
using ApiPeliculas.Models.Dtos;
using AutoMapper;

namespace ApiPeliculas.MoviesMappers
{
    public class MovieMapper : Profile
    {
        public MovieMapper()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Category, CreateCategoryDto>().ReverseMap();
            CreateMap<Movie, MovieDto>().ReverseMap();

        }
    }
}
