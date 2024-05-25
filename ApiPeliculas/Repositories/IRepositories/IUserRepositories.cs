using ApiPeliculas.Models;
using ApiPeliculas.Models.Dtos;

namespace ApiPeliculas.Repositories.IRepositories
{
    public interface IUserRepositories
    {
        ICollection<User> GetUsers();
        User GetUser(int userId);
        bool IsUniqueUser(string user);
        Task<UserLoginResponseDto> Login(UserLoginDto userLoginDto);
        Task<User> Register(UserRegisterDto userRegisterDto);
    }
}
