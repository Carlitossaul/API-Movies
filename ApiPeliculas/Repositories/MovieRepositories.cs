using ApiPeliculas.Data;
using ApiPeliculas.Models;
using ApiPeliculas.Repositories.IRepositories;

namespace ApiPeliculas.Repositories
{
    public class MovieRepositories : IMovieRepositories
    {
        private readonly ApplicationDbContext _db;
        public MovieRepositories(ApplicationDbContext db)
        {
            _db = db;
        }
        public bool CreateMovie(Movie movie)
        {
            movie.CreatedDate = DateTime.Now;
            _db.Movie.Add(movie);
            return Save();
        }

        public bool DeleteMovie(Movie movie)
        {
            var valor = _db.Movie.Remove(movie);
            return Save();
        }

        public bool ExistsMovie(string name)
        {
            var valor = _db.Movie.Any(movie => movie.Name.ToLower().Trim() == name.ToLower().Trim());
            return valor;
        }

        public bool ExistsMovie(int id)
        {
            var valor = _db.Movie.Any(movie => movie.Id == id);
            return valor;
        }

        public ICollection<Movie> GetMovies()
        {
            return _db.Movie.OrderBy(movie => movie.Name).ToList();
        }

        public Movie GetMovie(int movieId)
        {
            return _db.Movie.FirstOrDefault(movie => movie.Id == movieId);
        }

        public ICollection<Movie> GetMoviesByCategory(int categoryId)
        {
            return _db.Movie.Where(movie => movie.categoryId == categoryId).ToList();
        }

        public ICollection<Movie> GetMoviesByName(string name)
        {
            IQueryable<Movie> query = _db.Movie;
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(movie => movie.Name.Contains(name) || movie.Description.Contains(name));
            }
            return query.ToList();
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateMovie(Movie movie)
        {
            movie.CreatedDate = DateTime.Now;
            _db.Movie.Update(movie);
            return Save();
        }


    }
}
