using ApiPeliculas.Models;
using ApiPeliculas.Models.Dtos;
using ApiPeliculas.Repositories.IRepositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


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

        [AllowAnonymous]
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


        [AllowAnonymous]
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
            var itemMovieDto = _mapper.Map<Movie>(itemMovie);
            return Ok(itemMovieDto);
        }


        [Authorize(Roles = "admin")]
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(MovieDto))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateMovie([FromBody] MovieDto movieDto)
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


        [Authorize(Roles = "admin")]
        [HttpDelete("{movieId:int}", Name = "DeleteMovie")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeleteMovie(int movieId)
        {
            if (!_movieRepo.ExistsMovie(movieId))
            {
                return NotFound();
            }
            var movie = _movieRepo.GetMovie(movieId);
            if (!_movieRepo.DeleteMovie(movie))
            {
                ModelState.AddModelError("", $"Something was wrong deleting {movie.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();

        }

        [Authorize(Roles = "admin")]
        [HttpPatch("{movieId:int}", Name = "UpdateMovie")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateMovie(int movieId, [FromBody] MovieDto movieDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (movieDto == null || movieId != movieDto.Id)
            {
                return BadRequest(ModelState);
            }
            if (_movieRepo.ExistsMovie(movieDto.Name))
            {
                ModelState.AddModelError("", "Movie already exists");
                return StatusCode(404, ModelState);
            }

            var movie = _mapper.Map<Movie>(movieDto);
            if (!_movieRepo.UpdateMovie(movie))
            {
                ModelState.AddModelError("", $"Something was wrong updating {movie.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }


        [AllowAnonymous]
        [HttpGet("GetMoviesByCategory/{categoryId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public IActionResult GetMoviesByCategory(int categoryId)
        {
            var listMovies = _movieRepo.GetMoviesByCategory(categoryId);
            if (listMovies == null)
            {
                return NotFound();
            }
            var listMoviesDto = new List<MovieDto>();
            foreach (var list in listMovies)
            {
                listMoviesDto.Add(_mapper.Map<MovieDto>(list));
            }
            return Ok(listMoviesDto);
        }

        [AllowAnonymous]
        [HttpGet("GetMovieByName")]
        public IActionResult GetMovieByName(string name)
        {
            try
            {
                var result = _movieRepo.GetMoviesByName(name.Trim());
                if (result.Any())
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong");

            }
        }


    }
}
