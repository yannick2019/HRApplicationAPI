using AutoMapper;
using HRApplicationAPI.Interfaces;
using HRApplicationAPI.Models.DbModels;
using HRApplicationAPI.Models.InputModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRApplicationAPI.Controllers
{
    [Route("api/events")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IEventService _eventService;
        private readonly IMapper _mapper;
        private readonly ILogger<EventsController> _logger;

        public EventsController(
            IAuthService authService, 
            IEventService eventService,
            IMapper mapper, 
            ILogger<EventsController> logger)
        {
            _authService = authService;
            _eventService = eventService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("all")]
        //[Authorize]
        public ActionResult<Event[]> GetAll()
        {
            try
            {
                var events = _eventService.GetAll();
                return Ok(events);
            }
            catch (Exception error)
            {
                _logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<Event> GetById(string id)
        {
            try
            {
                var eventEntity = _eventService.GetById(id);

                if (eventEntity != null)
                {
                    return Ok(eventEntity);
                }

                return NotFound();
            }
            catch (Exception error)
            {
                _logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult<Event> Create(EventInputModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var mappedModel = _mapper.Map<EventInputModel, Event>(model);
                    var users = model.UserEmails.Select(x => _authService.GetByEmail(x));

                    mappedModel.UserEvents = users
                        .Select(x => new UserEvent() 
                        { 
                            EventId = model.Id, 
                            UserId = x!.UserId, 
                            User = x 
                        })
                        .ToList();

                    var eventEntity = _eventService.Create(mappedModel);
                    return Ok(eventEntity);
                }

                return BadRequest(ModelState);
            }
            catch (Exception error)
            {
                _logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [HttpPut]
        [Authorize(Roles = "Administrator")]
        public ActionResult<Event> Update(EventInputModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var mappedModel = _mapper.Map<EventInputModel, Event>(model);
                    var eventEntity = _eventService.Update(mappedModel);

                    if (eventEntity != null)
                    {
                        return Ok(eventEntity);
                    }

                    return NotFound();
                }

                return BadRequest(ModelState);
            }
            catch (Exception error)
            {
                _logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public ActionResult<Event> Delete(string id)
        {
            try
            {
                var eventEntity = _eventService.Delete(id);

                if (eventEntity != null)
                {
                    return Ok(eventEntity);
                }

                return NotFound();
            }
            catch (Exception error)
            {
                _logger.LogError(error.Message);
                return StatusCode(500);
            }
        }
    }
}
