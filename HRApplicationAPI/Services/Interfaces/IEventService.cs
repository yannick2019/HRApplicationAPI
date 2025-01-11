using HRApplicationAPI.Models.DbModels;

namespace HRApplicationAPI.Services.Interfaces
{
    public interface IEventService
    {
        public Event[] GetAll();
        public Event[] GetAllForUser(string email);
        public Event? GetById(string id);
        public Event Create(Event model);
        public Event? Update(Event model);
        public Event? Delete(string id);
    }
}
