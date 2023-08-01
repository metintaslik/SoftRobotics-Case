using AutoMapper;
using softrobotics.auth.application.UserHandler.Command;
using softrobotics.auth.domain.Entity;

namespace softrobotics.auth.application.Common.Mapping;

public class MapConfiguration : Profile
{
    public MapConfiguration()
    {
        CreateMap<User, CreateUserCommand>().ReverseMap();
    }
}