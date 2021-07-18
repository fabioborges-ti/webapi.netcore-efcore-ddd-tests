using System.Linq;
using Api.Data.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Integration.Test.Config
{
    public class ApiAppFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<DataContext>));

                services.Remove(descriptor);

                services.AddDbContext<DataContext>((options, context) =>
                {
                    context.UseSqlServer("Data Source=BRRIONW038466\\SQLEXPRESS;Initial Catalog=apinetcore;Integrated Security=True");
                });

                services.BuildServiceProvider();
            });
        }
    }
}
