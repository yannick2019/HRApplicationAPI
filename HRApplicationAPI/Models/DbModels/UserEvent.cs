namespace HRApplicationAPI.Models.DbModels
{
    public class UserEvent
    {
        public string UserId { get; set; } = string.Empty;
        public User? User { get; set; }

        public string EventId { get; set; } = string.Empty;
        public Event? Event { get; set; }
    }
}
