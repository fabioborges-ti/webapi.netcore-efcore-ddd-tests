using Api.Domain.Dtos.Login;
using Api.Domain.Interfaces.Repositories;
using Api.Domain.Interfaces.Services.Login;
using Api.Domain.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Api.Service.Services
{
    public class LoginService : ILoginService
    {
        private readonly ILogger<LoginService> _logger;
        private readonly IUserRepository _repository;
        private readonly SigningConfigurations _signingConfigurations;
        private readonly TokenConfigurations _tokenConfigurations;
        private readonly IConfiguration _configuration;

        public LoginService
        (
            ILogger<LoginService> logger, 
            IUserRepository repository, 
            SigningConfigurations signingConfigurations, 
            TokenConfigurations tokenConfigurations,
            IConfiguration configuration
        )
        {
            _logger = logger;
            _repository = repository;
            _signingConfigurations = signingConfigurations;
            _tokenConfigurations = tokenConfigurations;
            _configuration = configuration;
        }

        public async Task<object> FindByLogin(LoginDto user)
        {
            if (user == null || string.IsNullOrEmpty(user.Email))
            {
                _logger.LogError("Invalid email.");

                return null;
            }

            var result = await _repository.GetByEmail(user.Email);

            if (result == null)
            {
                _logger.LogInformation("Email not found");

                return new
                {
                    authenticated = false,
                    message = "unauthenticated user"
                };
            }

            if (!result.Status)
            {
                _logger.LogInformation("unauthorized user");

                return new
                {
                    authenticated = false,
                    message = "unauthorized user"
                };
            }

            var identity = new ClaimsIdentity
            (
                new GenericIdentity(user.Email), 
                new[] 
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.UniqueName, user.Email)
                }
            );

            var createdDate = DateTime.UtcNow;

            var expirationDate = createdDate + TimeSpan.FromSeconds(_tokenConfigurations.Seconds);

            var handler = new JwtSecurityTokenHandler();

            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _tokenConfigurations.Issuer,
                Audience = _tokenConfigurations.Audience,
                SigningCredentials = _signingConfigurations.SigningCredentials,
                Subject = identity,
                NotBefore = createdDate,
                Expires = expirationDate
            });

            var token = handler.WriteToken(securityToken);

            return new 
            {
                authenticated = true,
                created = createdDate.ToString("yyyy-MM-dd HH:mm:ss"),
                expiration = expirationDate.ToString("yyyy-MM-dd HH:mm:ss"),
                accessToken = token,
                userName = user.Email,
                message = "authenticated user"
            };
        }
    }
}