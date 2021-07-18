using Api.Data.Implementations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace Api.CrossCutting.DependencyInjection
{
    [ExcludeFromCodeCoverage]
    public static class ConfigureLogging
    {
        public static IServiceCollection ConfigureLoggingCollection(this IServiceCollection services)
        {
            services.AddSingleton<ILogger>(provider => provider.GetRequiredService<ILogger<AddressImplementations>>());
            services.AddSingleton<ILogger>(provider => provider.GetRequiredService<ILogger<PhoneImplementations>>());
            services.AddSingleton<ILogger>(provider => provider.GetRequiredService<ILogger<RegisterImplementations>>());
            services.AddSingleton<ILogger>(provider => provider.GetRequiredService<ILogger<UserImplementations>>());
            
            return services;
        }
    }
}
