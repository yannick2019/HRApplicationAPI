using AutoMapper;
using HRApplicationAPI.Models.DbModels;
using HRApplicationAPI.Models.InputModels;
using HRApplicationAPI.Models.OutputModels;

namespace HRApplicationAPI.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<LoginInputModel, User>().ReverseMap();
            CreateMap<RegisterInputModel, User>().ReverseMap();
            CreateMap<EventInputModel, Event>().ReverseMap();
            CreateMap<UserEventInputModel, UserEvent>().ReverseMap();
            CreateMap<User, UserOutputModel>().ReverseMap();
        }
    }
}
