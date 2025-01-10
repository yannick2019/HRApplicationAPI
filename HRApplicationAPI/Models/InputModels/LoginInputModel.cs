using System.ComponentModel.DataAnnotations;

namespace HRApplicationAPI.Models.InputModels
{
    public class LoginInputModel
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
