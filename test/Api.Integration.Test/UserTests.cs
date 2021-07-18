using Api.Application;
using Api.Domain.Dtos.User;
using Api.Integration.Test.Config;
using Api.Integration.Test.Dto;
using Bogus;
using Bogus.Extensions.Brazil;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Api.Integration.Test
{
    [Collection(nameof(IntegrationApiTestsFixtureCollection))]
    [TestCaseOrderer("Api.Integration.Test.PriorityOrderer", "Api.Integration.Test")]
    public class UserTests
    {
        private readonly IntegrationTestsFixture<Startup> _testsFixture;
        
        private readonly string _name;
        private readonly string _email;
        private readonly string _document;

        public UserTests(IntegrationTestsFixture<Startup> testsFixture)
        {
            _testsFixture = testsFixture;

            var faker = new Faker("pt_BR");

            _name = faker.Name.FullName();
            _email = faker.Internet.Email();
            _document = faker.Person.Cpf();
        }

        [Fact, TestPriority(1)]
        [Trait("UsersController", "Post")]
        public async Task Post()
        {
            await _testsFixture.GetToken();

            var token = JsonConvert.DeserializeObject<TokenDto>(_testsFixture.Token);

            _testsFixture.Client.SetToken(token.AccessToken);

            var userDtoCreate = new UserDtoCreate
            {
                Name = _name,
                Email = _email,
                Document = _document
            };

            var response = await _testsFixture.Client.PostAsync("api/users", new StringContent(JsonConvert.SerializeObject(userDtoCreate), Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode();

            var data = await response.Content.ReadAsStringAsync();

            var user = JsonConvert.DeserializeObject<UserDtoCreateResult>(data);

            _testsFixture.SetUserDtoCreateResult(user);

            // assert
            Assert.NotNull(user);
            Assert.IsType<UserDtoCreateResult>(user);
            Assert.Equal(userDtoCreate.Name, actual: user.Name);
            Assert.Equal(userDtoCreate.Email, user.Email);
            Assert.Equal(userDtoCreate.Document.Replace(".", "").Replace("-", ""), user.Document);
        }

        [Fact, TestPriority(2)]
        [Trait("UsersController", "Get")]
        public async Task Get()
        {
            // arrange
            var userDtoCreateResult = _testsFixture.GetUserDtoCreateResult();

            // act
            var response = await _testsFixture.Client.GetAsync($"api/users/{userDtoCreateResult.Id}");

            response.EnsureSuccessStatusCode();

            var data = await response.Content.ReadAsStringAsync();

            var user = JsonConvert.DeserializeObject<UserDto>(data);

            // assert
            Assert.NotNull(user);
            Assert.IsType<UserDto>(user);
            Assert.Equal(userDtoCreateResult.Name, actual: user.Name);
            Assert.Equal(userDtoCreateResult.Email, user.Email);
            Assert.Equal(userDtoCreateResult.Document.Replace(".", "").Replace("-", ""), user.Document);
            Assert.Equal(userDtoCreateResult.Status, user.Status);
        }

        [Fact, TestPriority(3)]
        [Trait("UsersController", "Patch")]
        public async Task Patch()
        {
            var userDtoCreateResult = _testsFixture.GetUserDtoCreateResult();

            var response = await _testsFixture.Client.PatchAsync($"api/users/changeStatus/user/{userDtoCreateResult.Id}", new StringContent(JsonConvert.SerializeObject(userDtoCreateResult), Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode();

            var data = await response.Content.ReadAsStringAsync();

            var user = JsonConvert.DeserializeObject<UserDto>(data);

            Assert.NotNull(user);
            Assert.IsType<UserDto>(user);
            Assert.Equal(userDtoCreateResult.Id, actual: user.Id);
            Assert.Equal(userDtoCreateResult.Name, actual: user.Name);
            Assert.Equal(userDtoCreateResult.Email, actual: user.Email);
            Assert.Equal(userDtoCreateResult.Status, !user.Status);
        }

        [Fact, TestPriority(4)]
        [Trait("UsersController", "Put")]
        public async Task Put()
        {
            // arrange
            var userDtoCreateResult = _testsFixture.GetUserDtoCreateResult();

            var userDtoUpdate = new UserDtoUpdate
            {
                Id = userDtoCreateResult.Id,
                Name = _name,                               // modify name 
                Email = _email,                             // modify email
                Document = _document,                       // modify document
                Status = userDtoCreateResult.Status
            };

            // act
            var response = await _testsFixture.Client.PutAsJsonAsync($"api/users/{userDtoCreateResult.Id}", userDtoUpdate);

            response.EnsureSuccessStatusCode();

            var data = await response.Content.ReadAsStringAsync();

            var user = JsonConvert.DeserializeObject<UserDtoUpdateResult>(data);

            // assert
            Assert.NotNull(user);
            Assert.IsType<UserDtoUpdateResult>(user);
            Assert.Equal(userDtoUpdate.Id, actual: user.Id);
            Assert.Equal(userDtoUpdate.Name, actual: user.Name);
            Assert.Equal(userDtoUpdate.Email, actual: user.Email);
            Assert.Equal(userDtoUpdate.Status, user.Status);
        }

        [Fact, TestPriority(5)]
        [Trait("UsersController", "Delete")]
        public async Task Delete()
        {
            // arrange
            var userDtoCreateResult = _testsFixture.GetUserDtoCreateResult();

            // act
            var response = await _testsFixture.Client.DeleteAsync($"api/users/{userDtoCreateResult.Id}");

            response.EnsureSuccessStatusCode();

            var data = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<bool>(data);

            // assert
            Assert.True(result);
        }

        [Fact, TestPriority(6)]
        [Trait("UsersController", "GetAll")]
        public async Task GetAll()
        {
            // act
            var response = await _testsFixture.Client.GetAsync($"api/users/");

            // assert
            response.EnsureSuccessStatusCode();
        }
    }
}
