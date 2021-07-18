using Api.Domain.Interfaces.Services.Register;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Api.Application.Controllers
{
    [Authorize("Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly ILogger<RegisterController> _logger;
        private readonly IRegisterService _service;

        public RegisterController(ILogger<RegisterController> logger, IRegisterService service)
        {
            _logger = logger;
            _service = service;
        }
        
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            try
            {
                var result = await _service.GetRegisters();

                return result == null ? NotFound() : Ok(result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(nameof(RegisterController), ex.Message, ex.StackTrace);

                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet("user/{id:guid}")]
        public async Task<ActionResult> GetById(Guid id)
        {
            try
            {
                var result = await _service.GetRegisterById(id);

                return result == null ? NotFound() : Ok(result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(nameof(RegisterController), ex.Message, ex.StackTrace);

                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
