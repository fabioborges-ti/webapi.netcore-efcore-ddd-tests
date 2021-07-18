using Api.Domain.Dtos.User;
using Api.Domain.Entities;
using Api.Domain.Interfaces;
using Api.Domain.Interfaces.Repositories;
using Api.Domain.Models;
using Api.Service.Notifications;
using Api.Service.Services;
using Api.Service.Test.Helpers;
using AutoMapper;
using Bogus;
using Bogus.Extensions.Brazil;
using ExpectedObjects;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;

namespace Api.Service.Test.IUserServiceTests
{
    public class UserServiceTest 
    {
        private readonly UserService _service;

        // MOCKS
        private readonly Mock<ILogger<UserService>> _logger = new();
        private readonly Mock<INotifier> _notifier = new();
        private readonly Mock<IMapper> _mapper = new();
        private readonly Mock<IRepository<UserEntity>> _userEntityRepository = new();
        private readonly Mock<IUserRepository> _userRepository = new();

        // FAKER
        private static readonly Guid FakerUserId = Guid.NewGuid();
        private static readonly DateTime FakerCreatedAt = DateTime.UtcNow;
        private static readonly Faker Faker = new("pt_BR");
        private static readonly string FakerName = Faker.Name.FullName();
        private static readonly string FakerEmail = Faker.Internet.Email();
        private static readonly string FakerDocument = Faker.Person.Cpf();

        public UserServiceTest()
        {
            _service = new UserService(_logger.Object, _notifier.Object, _mapper.Object, _userEntityRepository.Object, _userRepository.Object);
        }

        #region POST

        [Theory]
        [MemberData(nameof(PostSchemas))]
        [Trait(nameof(UserServiceTest), "POST")]
        public async void Post(PostModel testModel)
        {
            SetupMocks(testModel);

            var result = new UserDtoCreateResult();

            var exception = await Record.ExceptionAsync(async () =>
            {
                result = await _service.Post(testModel.Request);
            });

            TestLogHelpers.VerifyLogger(_logger, LogLevel.Information, null, Times.Exactly(testModel.LoggedInformationTimes));
            TestLogHelpers.VerifyLogger(_logger, LogLevel.Error, null, Times.Exactly(testModel.LoggedErrorTimes));

            if (exception != null)
            {
                Assert.Equal(testModel.ExceptionType, exception.GetType());

                return;
            }

            testModel.ExpectedResult.ToExpectedObject().ShouldEqual(result);
        }

        public static TheoryData<PostModel> PostSchemas = new()
        {
            new PostModel
            {
                Description = "Failed - invalid request [NULL]",
                Request = null,
                LoggedInformationTimes = 0,
                LoggedErrorTimes = 0,
                ExceptionType = typeof(ArgumentNullException)
            },
            new PostModel
            {
                Description = "Failed - invalid request [INVALID NAME]",
                Request = GetUserDtoCreate_Invalid_Name(string.Empty),
                LoggedInformationTimes = 0,
                LoggedErrorTimes = 1,
                ExpectedResult = null,
            },
            new PostModel
            {
                Description = "Failed - email already registered [EMAIL]",
                Request = GetUserDtoCreate(),
                GetByEmailResponse = GetUserEntity_EmailAlreadyRegistered(),
                LoggedInformationTimes = 0,
                LoggedErrorTimes = 1,
                ExpectedResult = null
            },
            new PostModel
            {
                Description = "Success - record saved successfully",
                Request = GetUserDtoCreate(),
                GetByEmailResponse = null,
                LoggedInformationTimes = 0,
                LoggedErrorTimes = 0,
                ExpectedResult = GetUserDtoCreateResult(),
            }
        };

        public class PostModel 
        {
            public string Description { get; set; }
            public UserDtoCreate Request { get; set; }
            public UserEntity GetByEmailResponse { get; set; }
            public UserDtoCreateResult ExpectedResult { get; set; }
            public int LoggedInformationTimes { get; set; }
            public int LoggedErrorTimes { get; set; }
            public Type ExceptionType { get; set; }
        }

        private void SetupMocks(PostModel testModel)
        {
            var userModel = new UserModel
            {
                Id = FakerUserId,
                CreatedAt = FakerCreatedAt,
                Name = FakerName,
                Email = FakerEmail,
                Document = FakerDocument,
                Status = true
            };

            var userEntity = new UserEntity(FakerName, FakerEmail, true)
            {
                Id = FakerUserId,
                CreatedAt = FakerCreatedAt,
                Document = FakerDocument,
                Status = true
            };

            _userRepository
                .Setup(c => c.GetByEmail(FakerEmail))
                .ReturnsAsync(testModel.GetByEmailResponse);

            _mapper
                .Setup(c => c.Map<UserModel>(testModel.Request))
                .Returns(userModel);
            
            _mapper
                .Setup(c => c.Map<UserEntity>(userModel))
                .Returns(userEntity);

            _userEntityRepository
                .Setup(c => c.InsertAsync(userEntity))
                .ReturnsAsync(userEntity);
            
            _mapper
                .Setup(c => c.Map<UserDtoCreateResult>(userEntity))
                .Returns(testModel.ExpectedResult);
        }

        #endregion

        #region PUT

        [Theory]
        [MemberData(nameof(PutSchemas))]
        [Trait(nameof(UserServiceTest), "PUT")]
        public async void Put(PutModel testModel)
        {
            SetupMocks(testModel);

            var result = new UserDtoUpdateResult();

            var exception = await Record.ExceptionAsync(async () =>
            {
                result = await _service.Put(testModel.Request);
            });

            TestLogHelpers.VerifyLogger(_logger, LogLevel.Information, null, Times.Exactly(testModel.LoggedInformationTimes));
            TestLogHelpers.VerifyLogger(_logger, LogLevel.Error, null, Times.Exactly(testModel.LoggedErrorTimes));

            if (exception != null)
            {
                Assert.Equal(testModel.ExceptionType, exception.GetType());
                
                return;
            }

            testModel.ExpectedResult.ToExpectedObject().ShouldEqual(result);
        }

        public static TheoryData<PutModel> PutSchemas = new()
        {
            new PutModel
            {
                Name = "Failed - invalid name",
                Request = GetUserDtoUpdate(string.Empty),
                SelectAsyncResponse = null,
                LoggedInformationTimes = 0,
                LoggedErrorTimes = 1,
                ExpectedResult = null
            },
            new PutModel
            {
                Name = "Failed - record not found",
                Request = GetUserDtoUpdate(FakerName),
                SelectAsyncResponse = null,
                LoggedInformationTimes = 0,
                LoggedErrorTimes = 0,
                ExpectedResult = null
            },
            new PutModel
            {
                Name = "Success - record updated successfully",
                Request = GetUserDtoUpdate(FakerName),
                SelectAsyncResponse = GetUserEntity(),
                LoggedInformationTimes = 1,
                LoggedErrorTimes = 0,
                ExpectedResult = GetUserDtoUpdateResult()
            }
        };

        public class PutModel
        {
            public string Name { get; set; }
            public UserDtoUpdate Request { get; set; }
            public int LoggedInformationTimes { get; set; }
            public int LoggedErrorTimes { get; set; }
            public UserEntity SelectAsyncResponse { get; set; }
            public UserDtoUpdateResult ExpectedResult { get; set; }
            public Type ExceptionType { get; set; }
        }

        private void SetupMocks(PutModel testModel)
        {
            var userModel = new UserModel
            {
                Name = FakerName,
                Email = FakerEmail,
                Document = FakerDocument
            };

            var userEntity = new UserEntity(FakerName, FakerEmail, true)
            {
                Id = FakerUserId,
                CreatedAt = FakerCreatedAt,
                Document = FakerDocument,
                Status = true
            };

            _mapper
                .Setup(c => c.Map<UserModel>(testModel.Request))
                .Returns(userModel);

            _mapper
                .Setup(c => c.Map<UserEntity>(userModel))
                .Returns(userEntity);

            _userEntityRepository
                .Setup(c => c.SelectAsync(userEntity.Id))
                .ReturnsAsync(testModel.SelectAsyncResponse);

            _userEntityRepository
                .Setup(c => c.UpdateAsync(userEntity))
                .ReturnsAsync(userEntity);

            _mapper
                .Setup(c => c.Map<UserDtoUpdateResult>(userEntity))
                .Returns(testModel.ExpectedResult);
        }

        #endregion

        #region DELETE

        [Theory]
        [MemberData(nameof(DeleteSchema))]
        [Trait(nameof(UserServiceTest), "DELETE")]
        public async void Delete(DeleteModel testModel)
        {
            SetupMocks(testModel);

            var result = false;

            var exception = await Record.ExceptionAsync(async () =>
            {
                result = await _service.Delete(testModel.Request);
            });

            TestLogHelpers.VerifyLogger(_logger, LogLevel.Information, null, Times.Exactly(testModel.LoggedInformationTimes));
            TestLogHelpers.VerifyLogger(_logger, LogLevel.Error, null, Times.Exactly(testModel.LoggedErrorTimes));

            if (exception != null)
            {
                Assert.Equal(testModel.ExceptionType, exception.GetType());
                
                return;
            }

            testModel.ExpectedResult.ToExpectedObject().ShouldEqual(result);
        }

        public static TheoryData<DeleteModel> DeleteSchema = new()
        {
            new DeleteModel
            {
                Description = "Failed - invalid UserId",
                Request = Guid.Empty,
                LoggedInformationTimes = 0,
                LoggedErrorTimes = 0,
                ExpectedResult = false
            },
            new DeleteModel
            {
                Description = "Failed - user not found",
                Request = FakerUserId,
                ExistAsyncResult = false,
                LoggedInformationTimes = 0,
                LoggedErrorTimes = 0,
                ExpectedResult = false
            },
            new DeleteModel
            {
                Description = "Success - record deleted successfully",
                Request = FakerUserId,
                ExistAsyncResult = true,
                LoggedInformationTimes = 1,
                LoggedErrorTimes = 0,
                ExpectedResult = true
            },
        };

        public class DeleteModel 
        {
            public string Description { get; set; }
            public Guid Request { get; set; }
            public bool ExpectedResult { get; set; }
            public bool ExistAsyncResult { get; set; }
            public int LoggedInformationTimes { get; set; }
            public int LoggedErrorTimes { get; set; }
            public Type ExceptionType { get; set; }
        }

        private void SetupMocks(DeleteModel testModel)
        {
            _userEntityRepository
                .Setup(c => c.ExistAsync(testModel.Request))
                .ReturnsAsync(testModel.ExistAsyncResult);

            _userEntityRepository
                .Setup(c => c.DeleteAsync(testModel.Request))
                .ReturnsAsync(true);
        }

        #endregion

        #region PATCH

        [Theory]
        [MemberData(nameof(PatchSchema))]
        [Trait(nameof(UserServiceTest), "PATCH")]
        public async void Patch(PatchModel testModel)
        {
            SetupMocks(testModel);

            var result = new UserDto();

            var exception = await Record.ExceptionAsync(async () =>
            {
                result = await _service.ChangeStatus(testModel.Request);
            });

            TestLogHelpers.VerifyLogger(_logger, LogLevel.Information, null, Times.Exactly(testModel.LoggedInformationTimes));
            TestLogHelpers.VerifyLogger(_logger, LogLevel.Error, null, Times.Exactly(testModel.LoggedErrorTimes));

            if (exception != null)
            {
                Assert.Equal(testModel.ExceptionType, exception.GetType());

                return;
            }

            if (result == null) return;

            testModel.ExpectedResult
                .ToExpectedObject(x => x.Ignore(i => i.CreatedAt))
                .ShouldEqual(result);
        }

        public static TheoryData<PatchModel> PatchSchema = new()
        {
            new PatchModel
            {
                Description = "Failed - invalid UserId",
                Request = Guid.Empty,
                SelectAsyncResponse = null,
                LoggedInformationTimes = 0,
                LoggedErrorTimes = 1,
                ExpectedResult = null
            },
            new PatchModel
            {
                Description = "Failed - invalid UserId",
                Request = FakerUserId,
                SelectAsyncResponse = null,
                LoggedInformationTimes = 1,
                LoggedErrorTimes = 0,
                ExpectedResult = null
            },
            new PatchModel
            {
                Description = "Success - status updated successfully",
                Request = FakerUserId,
                SelectAsyncResponse = GetUserEntity(),
                LoggedInformationTimes = 0,
                LoggedErrorTimes = 0,
                ExpectedResult = GetUserDto(false)
            },
            new PatchModel
            {
                Description = "Success - status updated successfully",
                Request = FakerUserId,
                SelectAsyncResponse = GetUserEntity(false),
                LoggedInformationTimes = 0,
                LoggedErrorTimes = 0,
                ExpectedResult = GetUserDto()
            }
        };

        public class PatchModel 
        {
            public string Description { get; set; }
            public Guid Request { get; set; }
            public UserDto ExpectedResult { get; set; }
            public UserEntity SelectAsyncResponse { get; set; }
            public int LoggedInformationTimes { get; set; }
            public int LoggedErrorTimes { get; set; }
            public Type ExceptionType { get; set; }
        }

        private void SetupMocks(PatchModel testModel)
        {
            _userEntityRepository
                .Setup(c => c.SelectAsync(testModel.Request))
                .ReturnsAsync(testModel.SelectAsyncResponse);

            UserModel userModel;

            if (testModel.SelectAsyncResponse != null)
            {
                var status = !testModel.SelectAsyncResponse.Status;

                userModel = new UserModel
                {
                    Id = testModel.SelectAsyncResponse.Id,
                    CreatedAt = testModel.SelectAsyncResponse.CreatedAt,
                    Name = testModel.SelectAsyncResponse.Name,
                    Email = testModel.SelectAsyncResponse.Email,
                    Document = testModel.SelectAsyncResponse.Document,
                    Status = status
                };
            }
            else
            {
                userModel = new UserModel
                {
                    Id = FakerUserId,
                    CreatedAt = FakerCreatedAt,
                    Name = FakerName,
                    Email = FakerEmail,
                    Document = FakerDocument,
                    Status = true
                };
            }

            var userEntity = new UserEntity(userModel.Name, userModel.Email, userModel.Status)
            {
                Id = userModel.Id,
                CreatedAt = userModel.CreatedAt,
                UpdatedAt = userModel.UpdatedAt,
                Document = userModel.Document
            };

            _mapper
                .Setup(c => c.Map<UserModel>(testModel.SelectAsyncResponse))
                .Returns(userModel);

            _mapper
                .Setup(c => c.Map<UserEntity>(userModel))
                .Returns(userEntity);

            _userEntityRepository
                .Setup(c => c.UpdateAsync(userEntity))
                .ReturnsAsync(userEntity);

            var userDto = new UserDto
            {
                Id = userEntity.Id,
                CreatedAt = userEntity.CreatedAt,
                Name = userEntity.Name,
                Email = userEntity.Email,
                Document = userEntity.Document,
                Status = userEntity.Status
            };

            _mapper
                .Setup(c => c.Map<UserDto>(userEntity))
                .Returns(userDto);
        }

        #endregion

        #region GET

        [Theory]
        [MemberData(nameof(GetSchema))]
        [Trait(nameof(UserServiceTest), "GET")]
        public async void Get(GetModel testModel)
        {
            SetupMocks(testModel);

            var result = new UserDto();

            var exception = await Record.ExceptionAsync(async () =>
            {
                result = await _service.Get(testModel.Request);
            });

            TestLogHelpers.VerifyLogger(_logger, LogLevel.Information, null, Times.Exactly(testModel.LoggedInformationTimes));
            TestLogHelpers.VerifyLogger(_logger, LogLevel.Error, null, Times.Exactly(testModel.LoggedErrorTimes));

            if (exception != null)
            {
                Assert.Equal(testModel.ExceptionType, exception.GetType());
            }

            testModel.ExpectedResult.ToExpectedObject().ShouldEqual(result);
        }

        public static TheoryData<GetModel> GetSchema = new()
        {
            new GetModel
            {
                Description = "Failed - invalid userId",
                Request = Guid.Empty,
                LoggedInformationTimes = 0,
                LoggedErrorTimes = 1,
                ExpectedResult = null
            },
            new GetModel
            {
                Description = "Failed - user not found",
                Request = FakerUserId,
                SelectAsyncResponse = null,
                LoggedInformationTimes = 1,
                LoggedErrorTimes = 0,
                ExpectedResult = null
            },
            new GetModel
            {
                Description = "success - get user",
                Request = FakerUserId,
                SelectAsyncResponse = GetUserEntity(),
                LoggedInformationTimes = 0,
                LoggedErrorTimes = 0,
                ExpectedResult = GetUserDto()
            }
        };

        public class GetModel 
        {
            public string Description { get; set; }
            public Guid Request { get; set; }
            public UserEntity SelectAsyncResponse { get; set; }
            public UserDto ExpectedResult { get; set; }
            public int LoggedInformationTimes { get; set; }
            public int LoggedErrorTimes { get; set; }
            public Type ExceptionType { get; set; }
        }

        private void SetupMocks(GetModel testModel)
        {
            _userEntityRepository
                .Setup(c => c.SelectAsync(testModel.Request))
                .ReturnsAsync(testModel.SelectAsyncResponse);

            _mapper
                .Setup(c => c.Map<UserDto>(testModel.SelectAsyncResponse))
                .Returns(testModel.ExpectedResult);
        }

        #endregion

        // PRIVATES METHODS =============

        private static UserDto GetUserDto(bool status = true)
        {
            return new()
            {
                Id = FakerUserId,
                CreatedAt = FakerCreatedAt,
                Name = FakerName,
                Email = FakerEmail,
                Document = FakerDocument.Replace(".", "").Replace("-", ""),
                Status = status
            };
        }

        private static UserDtoCreate GetUserDtoCreate()
        {
            return new()
            {
                Name = FakerName,
                Email = FakerEmail,
                Document = FakerDocument,
            };
        }

        private static UserDtoCreate GetUserDtoCreate_Invalid_Name(string name)
        {
            return new()
            {
                Name = name,
                Email = FakerEmail,
                Document = FakerDocument
            };
        }

        private static UserDtoCreateResult GetUserDtoCreateResult()
        {
            return new()
            {
                Id = FakerUserId,
                CreatedAt = FakerCreatedAt,
                Name = FakerName,
                Email = FakerEmail,
                Document = FakerDocument,
                Status = true
            };
        }

        private static UserDtoUpdate GetUserDtoUpdate(string name)
        {
            return new()
            {
                Id = FakerUserId,
                Name = name,
                Email = FakerEmail,
                Document = FakerDocument,
                Status = true
            };
        }

        private static UserDtoUpdateResult GetUserDtoUpdateResult()
        {
            return new()
            {
                Id = FakerUserId,
                Name = FakerName,
                Email = FakerEmail,
                Document = FakerDocument,
                UpdatedAt = DateTime.UtcNow,
                Status = true
            };
        }

        private static UserEntity GetUserEntity(bool status = true)
        {
            return new(FakerName, FakerEmail, status)
            {
                Id = FakerUserId,
                CreatedAt = FakerCreatedAt,
                Document = FakerDocument.Replace(".", "").Replace("-", "")
            };
        }

        private static UserEntity GetUserEntity_EmailAlreadyRegistered()
        {
            return new(FakerName, FakerEmail, true)
            {
                Id = FakerUserId,
                CreatedAt = FakerCreatedAt,
                Document = FakerDocument,
                Status = true
            };
        }
 
    }
}