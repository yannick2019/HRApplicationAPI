using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace HRApplicationAPI.Models.DbModels
{
    public class Event
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public IList<UserEvent> UserEvents { get; set; } = [];
    }
}
