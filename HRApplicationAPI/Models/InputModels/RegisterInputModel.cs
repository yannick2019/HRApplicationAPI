using System.ComponentModel.DataAnnotations;

namespace HRApplicationAPI.Models.InputModels
{
    public class RegisterInputModel
    {
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string ConfirmedPassword { get; set; } = string.Empty;
    }
}
