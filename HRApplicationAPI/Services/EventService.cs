using HRApplicationAPI.Data;
using HRApplicationAPI.Helpers;
using HRApplicationAPI.Models.DbModels;
using HRApplicationAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HRApplicationAPI.Services
{
    public class EventService : IEventService
    {
        private readonly DataContext _dataContext;

        public EventService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public Event[] GetAll()
        {
            return _dataContext.Events
                .Include(ev => ev.UserEvents)
                .ToArray();
        }

        public Event[] GetAllForUser(string email)
        {
            var user = _dataContext.Users
                .FirstOrDefault(user => user.Email == email);

            return _dataContext.Events
                .Include(ev => ev.UserEvents)
                .Where(e => e.UserEvents.FirstOrDefault(ue => ue.UserId == user!.UserId) != null)
                .ToArray();
        }

        public Event? GetById(string id)
        {
            return _dataContext.Events
                .Include(ev => ev.UserEvents)
                .FirstOrDefault(c => c.Id == id);
        }

        public Event Create(Event model)
        {
            var id = IdGenerator.CreateLetterId(6);
            var existWithId = GetById(id);

            while (existWithId != null)
            {
                id = IdGenerator.CreateLetterId(6);
                existWithId = GetById(id);
            }

            model.Id = id;
            var eventEntity = _dataContext.Events.Add(model);
            _dataContext.SaveChanges();

            return eventEntity.Entity;
        }

        public Event? Update(Event model)
        {
            var eventEntity = _dataContext.Events
                .Include(ev => ev.UserEvents)
                .FirstOrDefault(c => c.Id == model.Id);

            if (eventEntity != null)
            {
                eventEntity.Title = model.Title != null ? model.Title : eventEntity.Title;

                eventEntity.Date = model.Date;

                eventEntity.Category = model.Category != null ? model.Category : eventEntity.Category;

                eventEntity.UserEvents = model.UserEvents.Count! > 0 ? model.UserEvents : eventEntity.UserEvents;

                _dataContext.SaveChanges();
            }

            return eventEntity;
        }

        public Event? Delete(string id)
        {
            var eventEntity = GetById(id);

            if (eventEntity != null)
            {
                _dataContext.Events.Remove(eventEntity);
                _dataContext.SaveChanges();
            }

            return eventEntity;
        }
    }
}
