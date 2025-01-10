using System.ComponentModel.DataAnnotations;

namespace HRApplicationAPI.Models.InputModels
{
    public class UserEventInputModel
    {
        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public string EventId { get; set; } = string.Empty;
    }
}
