using HRApplicationAPI.Models.DbModels;

namespace HRApplicationAPI.Interfaces
{
    public interface IAuthService
    {
        public User RegisterUser(User model);
        public bool IsAuthenticated(string email, string password);
        public bool DoesUserExists(string email);
        public User[] GetAll();
        public User? GetById(string id);
        public User? GetByEmail(string email);
        public string GenerateJwtToken(string email, string role);
        public string? DecodeEmailFromToken(string token);
        public User? ChangeRole(string email, string role);
    }
}
