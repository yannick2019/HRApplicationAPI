using System.ComponentModel.DataAnnotations;

namespace HRApplicationAPI.Models.InputModels
{
    public class UserChangeRoleInputModel
    {
        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = string.Empty;
    }
}
