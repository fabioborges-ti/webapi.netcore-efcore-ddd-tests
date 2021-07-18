using Api.Application;
using Api.Domain.Dtos.Login;
using Api.Domain.Dtos.User;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace Api.Integration.Test.Config
{
    [CollectionDefinition(nameof(IntegrationApiTestsFixtureCollection))]
    public class IntegrationApiTestsFixtureCollection : ICollectionFixture<IntegrationTestsFixture<Startup>> { }

    public class IntegrationTestsFixture<TStartup> : IDisposable where TStartup : class
    {
        public readonly ApiAppFactory<TStartup> Factory;
        public HttpClient Client;
        public UserDtoCreateResult UserDtoCreateResult;
        public string Token;

        public IntegrationTestsFixture()
        {
            Factory = new ApiAppFactory<TStartup>();
            Client = Factory.CreateClient();
        }

        public void Dispose()
        {
            Client.Dispose();
            Factory.Dispose();
        }

        public async Task GetToken()
        {
            try
            {
                var loginDto = new LoginDto
                {
                    Email = "admin@apinetcore.com"
                };

                Client = Factory.CreateClient();

                var response = await Client.PostAsJsonAsync("api/login", loginDto);

                response.EnsureSuccessStatusCode();

                Token = await response.Content.ReadAsStringAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void SetUserDtoCreateResult(UserDtoCreateResult userDtoCreateResult)
        {
            UserDtoCreateResult = userDtoCreateResult;
        }

        public UserDtoCreateResult GetUserDtoCreateResult()
        {
            return UserDtoCreateResult;
        }
    }
}
