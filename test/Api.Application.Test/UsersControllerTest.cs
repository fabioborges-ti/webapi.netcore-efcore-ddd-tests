using Api.Application.Controllers;
using Api.Domain.Dtos.User;
using Api.Domain.Interfaces.Services.User;
using Api.Service.Notifications;
using Bogus;
using Bogus.Extensions.Brazil;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Api.Application.Test
{
    public class UsersControllerTest
    {
        private readonly UsersController _controller;

        private readonly Mock<ILogger<UsersController>> _logger = new();
        private readonly Mock<INotifier> _notifier = new();
        private readonly Mock<IUserService> _service = new();

        private readonly string _name;
        private readonly string _email;
        private readonly string _document;

        public UsersControllerTest()
        {
            _controller = new UsersController(_logger.Object, _notifier.Object, _service.Object);
            
            var faker = new Faker("pt_BR");

            _name = faker.Name.FullName();
            _email = faker.Internet.Email();
            _document = faker.Person.Cpf();
        }

        [Fact]
        [Trait("Application.Test", "UsersController_Post_ReturnCreatedResult")]
        public async Task UsersController_Post_ReturnCreatedResult()
        {
            // arrange
            var userDtoCreate = new UserDtoCreate
            {
                Name = _name,
                Email = _email,
                Document = _document
            };

            var userDtoCreateResult = new UserDtoCreateResult
            {
                Id = Guid.NewGuid(),
                Name = _name,
                Email = _email,
                CreatedAt = DateTime.UtcNow
            };

            _service
                .Setup(c => c.Post(It.IsAny<UserDtoCreate>()))
                .Returns(Task.FromResult(userDtoCreateResult));

            var url = new Mock<IUrlHelper>();

            url.Setup(x => x.Link(It.IsAny<string>(), It.IsAny<object>())).Returns("http://localhost:5000");

            _controller.Url = url.Object;

            // act
            var result = await _controller.Post(userDtoCreate);

            // assert
            Assert.True(_controller.ModelState.IsValid);
            Assert.True(result is CreatedResult);

            var resultValue = ((CreatedResult)result).Value as UserDtoCreateResult;

            Assert.NotNull(resultValue);
            Assert.Equal(userDtoCreate.Name, resultValue.Name);
            Assert.Equal(userDtoCreate.Email, resultValue.Email);

            _service.Verify(c => c.Post(It.IsAny<UserDtoCreate>()), Times.Once);
        }

        [Fact]
        [Trait("Application.Test", "UsersController_Post_ReturnBadRequestObjectResult")]
        public async Task UsersController_Post_ReturnBadRequestObjectResult()
        {
            // arrange
            var userDtoCreate = new UserDtoCreate
            {
                Name = _name,
                Email = _email,
                Document = _document
            };

            var userDtoCreateResult = new UserDtoCreateResult
            {
                Id = Guid.NewGuid(),
                Name = _name,
                Email = _email,
                Document = _document,
                CreatedAt = DateTime.UtcNow
            };

            _service
                .Setup(c => c.Post(It.IsAny<UserDtoCreate>()))
                .Returns(Task.FromResult(userDtoCreateResult));

            _controller.ModelState.AddModelError("Name", "Name is required"); // adding error

            var url = new Mock<IUrlHelper>();

            url
                .Setup(x => x.Link(It.IsAny<string>(), It.IsAny<object>()))
                .Returns("http://localhost:5000");

            _controller.Url = url.Object;

            // act
            var result = await _controller.Post(userDtoCreate);

            // assert
            Assert.True(!_controller.ModelState.IsValid);
            Assert.True(result is BadRequestObjectResult);
            
            _service.Verify(c => c.Post(It.IsAny<UserDtoCreate>()), Times.Never);
        }

        [Fact]
        [Trait("Application.Test", "UsersController_Put_ReturnOkObjectResult")]
        public async Task UsersController_Put_ReturnOkObjectResult()
        {
            // arrange
            var userId = Guid.NewGuid();

            var userDtoUpdate = new UserDtoUpdate
            {
                Id = userId,
                Name = _name,
                Email = _email,
            };

            var userDtoUpdateResult = new UserDtoUpdateResult
            {
                Id = userId,
                Name = _name,
                Email = _email,
                UpdatedAt = DateTime.UtcNow
            };

            _service
                .Setup(c => c.Put(It.IsAny<UserDtoUpdate>()))
                .Returns(Task.FromResult(userDtoUpdateResult));

            var url = new Mock<IUrlHelper>();

            url
                .Setup(x => x.Link(It.IsAny<string>(), It.IsAny<object>()))
                .Returns("http://localhost:5000");

            _controller.Url = url.Object;

            // act
            var result = await _controller.Put(userId, userDtoUpdate);

            // assert
            Assert.True(_controller.ModelState.IsValid);
            Assert.True(result is OkObjectResult);

            var resultValue = ((OkObjectResult)result).Value as UserDtoUpdateResult;

            Assert.NotNull(resultValue);

            Assert.Equal(userDtoUpdate.Id, resultValue.Id);
            Assert.Equal(userDtoUpdate.Name, resultValue.Name);
            Assert.Equal(userDtoUpdate.Email, resultValue.Email);

            _service.Verify(c => c.Put(It.IsAny<UserDtoUpdate>()), Times.Once);
        }

        [Fact]
        [Trait("Application.Test", "UsersController_Put_ReturnBadRequest")]
        public async Task UsersController_Put_ReturnBadRequest()
        {
            // arrange
            var userId = Guid.NewGuid();

            var userDtoUpdate = new UserDtoUpdate
            {
                Id = userId,
                Name = _name,
                Email = _email
            };

            var userDtoUpdateResult = new UserDtoUpdateResult
            {
                Id = userId,
                Name = _name,
                Email = _email,
                UpdatedAt = DateTime.UtcNow
            };

            _service
                .Setup(c => c.Put(It.IsAny<UserDtoUpdate>()))
                .Returns(Task.FromResult(userDtoUpdateResult));

            _controller.ModelState.AddModelError("Name", "Name is required"); // adding error

            var url = new Mock<IUrlHelper>();

            url
                .Setup(x => x.Link(It.IsAny<string>(), It.IsAny<object>()))
                .Returns("http://localhost:5000");

            _controller.Url = url.Object;

            // act
            var result = await _controller.Put(userId, userDtoUpdate);

            // assert
            Assert.True(result is BadRequestObjectResult);
            Assert.True(!_controller.ModelState.IsValid);

            _service.Verify(c => c.Post(It.IsAny<UserDtoCreate>()), Times.Never);
        }

        [Fact]
        [Trait("Application.Test", "UsersController_Delete_ReturnOkObjectResult")]
        public async Task UsersController_Delete_ReturnOkObjectResult()
        {
            // arrange
            _service
                .Setup(c => c.Delete(It.IsAny<Guid>()))
                .Returns(Task.FromResult(true));

            var url = new Mock<IUrlHelper>();

            url
                .Setup(x => x.Link(It.IsAny<string>(), It.IsAny<object>()))
                .Returns("http://localhost:5000");

            _controller.Url = url.Object;

            // act
            var result = await _controller.Delete(Guid.NewGuid());

            // assert
            Assert.True(_controller.ModelState.IsValid);
            Assert.True(result is OkObjectResult);

            var resultValue = ((OkObjectResult)result).Value;

            Assert.NotNull(resultValue);
            Assert.True((bool)resultValue);

            _service.Verify(c => c.Delete(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        [Trait("Application.Test", "UsersController_Delete_ReturnBadRequest")]
        public async Task UsersController_Delete_ReturnBadRequest()
        {
            // arrange
            _service
                .Setup(c => c.Delete(It.IsAny<Guid>()))
                .Returns(Task.FromResult(true));

            var url = new Mock<IUrlHelper>();

            url
                .Setup(x => x.Link(It.IsAny<string>(), It.IsAny<object>()))
                .Returns("http://localhost:5000");

            _controller.Url = url.Object;
            _controller.ModelState.AddModelError("Id", "Invalid format.");

            // act
            var result = await _controller.Delete(default);

            // assert
            Assert.True(result is BadRequestObjectResult);
            Assert.True(!_controller.ModelState.IsValid);

            _service.Verify(c => c.Delete(It.IsAny<Guid>()), Times.Never);
        }

        [Fact]
        [Trait("Application.Test", "UsersController_Get_ReturnOkObjectResult")]
        public async Task UsersController_Get_ReturnOkObjectResult()
        {
            // arrange
            var userDto = new UserDto
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                Name = _name,
                Email = _email
            };

            _service
                .Setup(c => c.Get(It.IsAny<Guid>()))
                .Returns(Task.FromResult(userDto));

            var url = new Mock<IUrlHelper>();

            url
                .Setup(x => x.Link(It.IsAny<string>(), It.IsAny<object>()))
                .Returns("http://localhost:5000");

            _controller.Url = url.Object;

            // act
            var result = await _controller.Get(Guid.NewGuid());

            // assert
            Assert.True(_controller.ModelState.IsValid);
            Assert.True(result is OkObjectResult);

            var resultValue = ((OkObjectResult)result).Value as UserDto;

            Assert.NotNull(resultValue);

            Assert.Equal(userDto.Id, resultValue.Id);
            Assert.Equal(userDto.Name,  resultValue.Name);
            Assert.Equal(userDto.Email, resultValue.Email);

            _service.Verify(c => c.Get(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        [Trait("Application.Test", "UsersController_Get_ReturnBadRequestResult")]
        public async Task UsersController_Get_ReturnBadRequestResult()
        {
            // arrange
            var userDto = new UserDto
            {
                Id = Guid.NewGuid(),
                Name = _name,
                Email = _email,
                Document = _document,
                CreatedAt = DateTime.UtcNow
            };

            _service
                .Setup(c => c.Get(It.IsAny<Guid>()))
                .Returns(Task.FromResult(userDto));

            _controller.ModelState.AddModelError("Id", "Invalid format");

            var url = new Mock<IUrlHelper>();

            url
                .Setup(x => x.Link(It.IsAny<string>(), It.IsAny<object>()))
                .Returns("http://localhost:5000");

            _controller.Url = url.Object;

            // act
            var result = await _controller.Get(Guid.NewGuid());

            // assert
            Assert.True(!_controller.ModelState.IsValid);

            _service.Verify(c => c.Get(It.IsAny<Guid>()), Times.Never);
        }

        [Fact]
        [Trait("Application.Test", "UsersController_GetAll_ReturnOkObjectResult")]
        public async Task UsersController_GetAll_ReturnOkObjectResult()
        {
            // arrange
            var listUsers = new List<UserDto>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = _name,
                    Email = _email,
                    CreatedAt = DateTime.UtcNow
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = _name,
                    Email = _email,
                    CreatedAt = DateTime.UtcNow
                }
            };

            _service
                .Setup(c => c.GetAll())
                .ReturnsAsync(listUsers);

            var url = new Mock<IUrlHelper>();

            url
                .Setup(x => x.Link(It.IsAny<string>(), It.IsAny<object>()))
                .Returns("http://localhost:5000");

            _controller.Url = url.Object;

            // act
            var result = await _controller.GetAll();

            // assert
            Assert.True(_controller.ModelState.IsValid);
            Assert.True(result is OkObjectResult);

            var resultValue = ((OkObjectResult)result).Value as IEnumerable<UserDto>;

            Assert.NotNull(resultValue);
            Assert.True(resultValue.Any());

            _service.Verify(c => c.GetAll(), Times.Once);
        }

        [Fact]
        [Trait("Application.Test", "UsersController_GetAll_ReturnBadRequestResult")]
        public async Task UsersController_GetAll_ReturnBadRequestResult()
        {
            // arrange
            var listUsers = new List<UserDto>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = _name,
                    Email = _email,
                    CreatedAt = DateTime.UtcNow
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = _name,
                    Email = _email,
                    CreatedAt = DateTime.UtcNow
                }
            };

            _service
                .Setup(c => c.GetAll())
                .ReturnsAsync(listUsers);

            _controller.ModelState.AddModelError("Id", "Invalid Format");

            var url = new Mock<IUrlHelper>();

            url
                .Setup(x => x.Link(It.IsAny<string>(), It.IsAny<object>()))
                .Returns("http://localhost:5000");

            _controller.Url = url.Object;

            // act
            var result = await _controller.GetAll();

            // assert
            Assert.True(!_controller.ModelState.IsValid);
            Assert.True(result is BadRequestObjectResult);

            _service.Verify(c => c.GetAll(), Times.Never);
        }
    }
}
