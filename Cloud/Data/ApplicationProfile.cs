using AutoMapper;
using Cloud.DTOs;
using Cloud.Models;

namespace Cloud.Data;

public class ApplicationProfile : Profile
{
    public ApplicationProfile()
    {

        CreateMap<UserDTOs, User>();
        CreateMap<User, UserDTOs>();

        CreateMap<UserForCreate, User>();
        CreateMap<User, UserForCreate>();
        
    }
}