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

        CreateMap<UserCreateDTO, User>();
        CreateMap<User, UserCreateDTO>();

        CreateMap<User, UserEditDTO>();
        CreateMap<UserEditDTO, User>();

        CreateMap<FileRecord, FileDTOs>();
        CreateMap<FileDTOs, FileRecord>();

    }
}