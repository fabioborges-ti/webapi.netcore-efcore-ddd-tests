using Api.Data.Context;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Diagnostics.CodeAnalysis;

namespace Api.CrossCutting.HealthCheck
{
    [ExcludeFromCodeCoverage]
    public static class AppHealthCheck
    {
        public static HealthCheckResult Check(IServiceCollection service)
        {
            service
                .AddHealthChecks()
                .AddCheck("Server Health Check", () => HealthCheckResult.Healthy("Server is healthy"))
                .AddDbContextCheck<DataContext>();

            return HealthCheckResult.Healthy();
        }
    }
}
