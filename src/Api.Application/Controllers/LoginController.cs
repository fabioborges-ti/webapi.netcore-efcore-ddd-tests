using Api.Domain.Dtos.Login;
using Api.Domain.Interfaces.Services.Login;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Api.Application.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        private readonly ILoginService _service;

        public LoginController(ILogger<LoginController> logger, ILoginService service)
        {
            _logger = logger;
            _service = service;
        }
        
        [HttpPost]
        public async Task<ActionResult> Login([FromBody] LoginDto login)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (login == null) return BadRequest(ModelState);

            try
            {
                var result = await _service.FindByLogin(login);

                return result != null ? Ok(result) : NotFound();
            }
            catch (ArgumentException ex)
            {
                _logger.LogError($"{nameof(LoginController)} -> Error method -> [{nameof(Login)}];[{ex.Message}];[{ex.StackTrace}]");

                return StatusCode((int) HttpStatusCode.InternalServerError, ex.Message);
            }
        } 
    }
}
