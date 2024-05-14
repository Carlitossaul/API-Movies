using ApiPeliculas.Models;
using ApiPeliculas.Models.Dtos;
using ApiPeliculas.Repositories.IRepositories;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiPeliculas.Controllers
{
    [Route("api/movies")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieRepositories _movieRepo;
        private readonly IMapper _mapper;
        public MoviesController(IMovieRepositories movieRepo, IMapper mapper)
        {
            _movieRepo = movieRepo;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetMovies()
        {
            var listMovies = _movieRepo.GetMovies();
            var listMoviesDto = new List<MovieDto>();

            foreach (var list in listMovies)
            {
                listMoviesDto.Add(_mapper.Map<MovieDto>(list));
            }

            return Ok(listMoviesDto);
        }

        [HttpGet("{movieId:int}", Name = "GetMovie")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetMovie(int movieId)
        {
            var itemMovie = _movieRepo.GetMovie(movieId);

            if (itemMovie == null)
            {
                return NotFound();
            }
            var itemMovieDto = _mapper.Map<CategoryDto>(itemMovie);
            return Ok(itemMovieDto);
        }

        [HttpPost]

        [ProducesResponseType(201, Type = typeof(MovieDto))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public IActionResult CreateMovie([FromBody] MovieDto movieDto )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (movieDto == null)
            {
                return BadRequest(ModelState);
            }
            if (_movieRepo.ExistsMovie(movieDto.Name))
            {
                ModelState.AddModelError("", "Movie already exists");
                return StatusCode(404, ModelState);
            }

            var movie = _mapper.Map<Movie>(movieDto);
            if (!_movieRepo.CreateMovie(movie))
            {
                ModelState.AddModelError("", $"Something was wrong {movie.Name}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetMovie", new { movieId = movie.Id }, movie);
        }

    }
}
