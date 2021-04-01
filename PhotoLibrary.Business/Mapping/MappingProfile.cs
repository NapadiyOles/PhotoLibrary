using AutoMapper;
using PhotoLibrary.Business.Models;
using PhotoLibrary.Data.Entities;

namespace PhotoLibrary.Business.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserDTO, User>().ConvertUsing<UserConverter>();

            CreateMap<User, UserDTO>().ForMember(d => d.Name,
                opt => opt.MapFrom(s => s.UserName));
            
            CreateMap<PictureDTO, Picture>().ReverseMap();
        }
    }
}