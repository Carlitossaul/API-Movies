using ApiPeliculas.Models;

namespace ApiPeliculas.Repositories.IRepositories
{
    public interface IMovieRepositories
    {
        ICollection<Movie> GetMovies();
        Movie GetMovie(int movieId);
        bool ExistsMovie(string name);
        bool ExistsMovie(int id);
        bool CreateMovie(Movie movie);
        bool UpdateMovie(Movie movie);
        bool DeleteMovie(Movie movie);
        ICollection<Movie> GetMoviesByCategory(int categoryId);
        ICollection<Movie> GetMoviesByName(string name);
        bool Save();
    }
}
