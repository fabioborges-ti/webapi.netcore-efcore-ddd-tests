using Api.Domain.Interfaces.Services.Address;
using Api.Domain.Interfaces.Services.Login;
using Api.Domain.Interfaces.Services.Phone;
using Api.Domain.Interfaces.Services.Register;
using Api.Domain.Interfaces.Services.User;
using Api.Service.Notifications;
using Api.Service.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Api.CrossCutting.DependencyInjection
{
    [ExcludeFromCodeCoverage]
    public static class ConfigureService
    {
        public static IServiceCollection ConfigureDependenciesService(this IServiceCollection services)
        {
            services.AddScoped<INotifier, Notifier>();

            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<IPhoneService, PhoneService>();
            services.AddScoped<IRegisterService, RegisterService>();

            return services;
        }
    }
}
