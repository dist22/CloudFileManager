using AutoMapper;
using Cloud.Application.DTOs.File;
using Cloud.Application.DTOs.User;
using Cloud.Domain.Models;

namespace Cloud.Application.ApplicationProfile;

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