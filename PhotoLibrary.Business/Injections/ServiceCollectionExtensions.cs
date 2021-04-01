using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using PhotoLibrary.Business.Interfaces;
using PhotoLibrary.Business.Mapping;
using PhotoLibrary.Business.Services;
using PhotoLibrary.Data.Injections;

namespace PhotoLibrary.Business.Injections
{
    public static class ServiceCollectionExtensions
    {
        public static void AddBusinessServices(this IServiceCollection services, string connectionString)
        {
            services.AddUnitOfWork(connectionString);

            services.AddSingleton<IMapper>(new Mapper(new MapperConfiguration(cfg => 
                cfg.AddProfile<MappingProfile>())));

            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IPictureService, PictureService>();
        }
    }
}