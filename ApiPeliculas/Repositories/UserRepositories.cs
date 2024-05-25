using ApiPeliculas.Data;
using ApiPeliculas.Models;
using ApiPeliculas.Models.Dtos;
using ApiPeliculas.Repositories.IRepositories;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using XSystem.Security.Cryptography;

namespace ApiPeliculas.Repositories
{
    public class UserRepositories : IUserRepositories

    {
        private readonly ApplicationDbContext _db;
        private string keySecret;

        public UserRepositories(ApplicationDbContext db, IConfiguration config)
        {
            _db = db;
            keySecret = config.GetValue<string>("ApiSettings:Secret");
        }
        public User GetUser(int userId)
        {
            return _db.User.FirstOrDefault(User => User.Id == userId);
        }

        public ICollection<User> GetUsers()
        {
            return _db.User.OrderBy(User => User.Name).ToList();
        }

        public bool IsUniqueUser(string user)
        {
            var userExists = _db.User.Any(User => User.Name.ToLower().Trim() == user.ToLower().Trim());
            if(!userExists)
            {
                return true;
            }
            return false;
        }

        public async Task<UserLoginResponseDto> Login(UserLoginDto userLoginDto)
        {
            var passwordHash = getmd5(userLoginDto.Password);

            var user = _db.User.FirstOrDefault(User => User.UserName == userLoginDto.UserName && User.Password == passwordHash);

            if (user == null)
            {
                return new UserLoginResponseDto()
                {
                    User = null,
                    Token = ""
                 
                };
            }

            var handleToken = new JwtSecurityTokenHandler();


            var key = Encoding.ASCII.GetBytes(keySecret);


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = handleToken.CreateToken(tokenDescriptor);

            user.Password = "";

            return new UserLoginResponseDto()
            {
                User = user,
                Token = handleToken.WriteToken(token)
            };

        }

    public async Task<User> Register(UserRegisterDto userRegisterDto)
    {
        var passwordHash = getmd5(userRegisterDto.Password);

        User user = new User()
        {
            Name = userRegisterDto.Name,
            UserName = userRegisterDto.UserName,
            Password = passwordHash,
            Role = userRegisterDto.Role
        };

        _db.User.Add(user);
        await _db.SaveChangesAsync();
        return user;

    }


    public static string getmd5(string valor)
    {
        MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
        byte[] data = System.Text.Encoding.UTF8.GetBytes(valor);
        data = x.ComputeHash(data);
        string resp = "";
        for (int i = 0; i < data.Length; i++)
            resp += data[i].ToString("x2").ToLower();
        return resp;
    }
}
}
