using ApiPeliculas.Models;
using ApiPeliculas.Models.Dtos;
using ApiPeliculas.Repositories.IRepositories;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ApiPeliculas.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepositories _userRepo;
        protected ResponseAPI _responseAPI;
        private readonly IMapper _mapper;
        public UsersController(IUserRepositories userRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            this._responseAPI = new();
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public IActionResult GetUsers()
        {
            var listUsers = _userRepo.GetUsers();
            var listUsersDTO = new List<UserDto>();
            foreach (var list in listUsers)
            {
                listUsersDTO.Add(_mapper.Map<UserDto>(list));
            }
            return Ok(listUsersDTO);
        }

        [HttpGet("{userId:int}", Name = "GetUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetUser(int userId)
        {
            var itemUser = _userRepo.GetUser(userId);

            if (itemUser == null)
            {
                return NotFound();
            }
            var itemUserDto = _mapper.Map<CategoryDto>(itemUser);
            return Ok(itemUserDto);
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegisterDto)
        {
            bool isUnique = _userRepo.IsUniqueUser(userRegisterDto.UserName);
            if (isUnique == false)
            {
                _responseAPI.StatusCode = HttpStatusCode.BadRequest;
                _responseAPI.IsSuccess = false;
                _responseAPI.ErrorMessages = new List<string>() { "UserName already exists" };
                return BadRequest(_responseAPI);
            }

            var user = await _userRepo.Register(userRegisterDto);
            if (user == null)
            {
                _responseAPI.StatusCode = HttpStatusCode.InternalServerError;
                _responseAPI.IsSuccess = false;
                _responseAPI.ErrorMessages = new List<string>() { "Something went wrong" };
                return StatusCode(500, _responseAPI);
            }
            _responseAPI.StatusCode = HttpStatusCode.OK;
            _responseAPI.IsSuccess = true;
            return Ok(_responseAPI);

        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
        {

            var responseDto = await _userRepo.Login(userLoginDto);

            if (responseDto.User == null || string.IsNullOrEmpty(responseDto.Token))
            {
                _responseAPI.StatusCode = HttpStatusCode.BadRequest;
                _responseAPI.IsSuccess = false;
                _responseAPI.ErrorMessages = new List<string>() { "UserName or password is invalid" };
                return BadRequest(_responseAPI);
            }

            _responseAPI.StatusCode = HttpStatusCode.OK;
            _responseAPI.IsSuccess = true;
            _responseAPI.Result = responseDto;
            return Ok(_responseAPI);

        }

    }
}
