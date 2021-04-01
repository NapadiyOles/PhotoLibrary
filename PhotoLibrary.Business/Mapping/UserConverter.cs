using AutoMapper;
using PhotoLibrary.Business.Models;
using PhotoLibrary.Data.Entities;

namespace PhotoLibrary.Business.Mapping
{
    public class UserConverter : ITypeConverter<UserDTO, User>
    {
        public User Convert(UserDTO source, User destination, ResolutionContext context) =>
            new() {UserName = source.Name, Email = source.Email};
    }
}