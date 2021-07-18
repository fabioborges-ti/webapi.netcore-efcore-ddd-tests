using Api.Data.Context;
using Api.Data.Implementations;
using Api.Data.Repositories;
using Api.Domain.Interfaces;
using Api.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Api.CrossCutting.DependencyInjection
{
    [ExcludeFromCodeCoverage]
    public static class ConfigureRepository
    {
        public static IServiceCollection ConfigureDependenciesRepository(this IServiceCollection service, string connectionString)
        {
            service.AddDbContext<DataContext>(options => 
                options.UseSqlServer(connectionString));

            service.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
            
            service.AddScoped<IUserRepository, UserImplementations>();
            service.AddScoped<IAddressRepository, AddressImplementations>();
            service.AddScoped<IPhoneRepository, PhoneImplementations>();
            service.AddScoped<IRegisterRepository, RegisterImplementations>();

            return service;
        }
    }
}
