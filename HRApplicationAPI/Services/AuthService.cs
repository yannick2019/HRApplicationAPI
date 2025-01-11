using HRApplicationAPI.Data;
using HRApplicationAPI.Helpers;
using HRApplicationAPI.Models.DbModels;
using HRApplicationAPI.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BC = BCrypt.Net.BCrypt;

namespace HRApplicationAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly DataContext _dataContext;

        public AuthService(DataContext dataContext, IConfiguration configuration)
        {
            _configuration = configuration;
            _dataContext = dataContext;
        }

        public User RegisterUser(User model)
        {
            var id = IdGenerator.CreateLetterId(10);
            var existWithId = GetById(id);

            while (existWithId != null)
            {
                id = IdGenerator.CreateLetterId(10);
                existWithId = GetById(id);
            }

            model.UserId = id;
            model.Password = BC.HashPassword(model.Password);
            var userEntity = _dataContext.Users.Add(model);
            _dataContext.SaveChanges();

            return userEntity.Entity;
        }

        public bool IsAuthenticated(string email, string password)
        {
            var user = GetByEmail(email);
            return DoesUserExists(email) && BC.Verify(password, user!.Password);
        }

        public bool DoesUserExists(string email)
        {
            var user = _dataContext.Users.FirstOrDefault(x => x.Email == email);
            return user != null;
        }

        public User[] GetAll()
        {
            return _dataContext.Users.ToArray();
        }

        public User? GetById(string id)
        {
            return _dataContext.Users.FirstOrDefault(c => c.UserId == id);
        }

        public User? GetByEmail(string email)
        {
            return _dataContext.Users.FirstOrDefault(c => c.Email == email);
        }

        public string GenerateJwtToken(string email, string role)
        {
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                    {
                        new Claim("Id", Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Sub, email),
                        new Claim(JwtRegisteredClaimNames.Email, email),
                        new Claim(ClaimTypes.Role, role),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    }),
                Expires = DateTime.UtcNow.AddMinutes(5),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public string? DecodeEmailFromToken(string token)
        {
            var decodedToken = new JwtSecurityTokenHandler();
            var indexOfTokenValue = 7;
            var t = decodedToken.ReadJwtToken(token.Substring(indexOfTokenValue));
            return t.Payload.FirstOrDefault(x => x.Key == "email").Value.ToString();
        }

        public User? ChangeRole(string email, string role)
        {
            var user = GetByEmail(email);
            if (user != null)
            {
                user.Role = role;
            }
       
            _dataContext.SaveChanges();

            return user;
        }
    }
}
