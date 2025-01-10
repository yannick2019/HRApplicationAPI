namespace HRApplicationAPI.Models.InputModels
{
    public class EventInputModel
    {
        public string Id { get; set; } = string.Empty;
        public string? Title { get; set; } 
        public string? Category { get; set; } 
        public DateTime Date { get; set; }

        public IList<string> UserEmails { get; set; }
    }
}
