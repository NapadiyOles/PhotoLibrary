using AutoMapper;
using PhotoLibrary.Api.Models.Picture;
using PhotoLibrary.Api.Models.User;
using PhotoLibrary.Business.Models;

namespace PhotoLibrary.Api.Mapping
{
    public class ApiMappingProfile : Profile
    {
        public ApiMappingProfile()
        {
            CreateMap<PictureDTO, PictureViewModel>();
            CreateMap<PictureCreateModel, PictureDTO>();
            CreateMap<UserRegisterModel, UserDTO>();
            CreateMap<UserLoginModel, UserDTO>();
            CreateMap<UserDTO, UserViewModel>();
        }
    }
}