using Api.Application.Custom;
using Api.Domain.Dtos.User;
using Api.Domain.Interfaces.Services.User;
using Api.Service.Notifications;
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
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly INotifier _notifier;
        private readonly IUserService _service;

        public UsersController(ILogger<UsersController> logger, INotifier notifier, IUserService userService)
        {
            _logger = logger;
            _notifier = notifier;
            _service = userService;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] UserDtoCreate user)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var result = await _service.Post(user);

                if (_notifier.HasNotification())
                {
                    return BadRequest(Responses.GetErrors(_notifier.GetNotification()));
                }

                if (result == null)
                {
                    return BadRequest();
                }

                _logger.LogInformation($"Record created successfully.");

                return Created(new Uri(Url.Link("GetWithId", new { id = result.Id }) ?? string.Empty), result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(nameof(UsersController), ex.Message, ex.StackTrace);

                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Put(Guid id, [FromBody] UserDtoUpdate user)
        {
            if (id != user.Id) 
                return BadRequest(Responses.GetErrors("UserId is not same than UserDtoUpdate."));

            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var result = await _service.Put(user);

                if (_notifier.HasNotification())
                {
                    return BadRequest(Responses.GetErrors(_notifier.GetNotification()));
                }

                if (result == null)
                {
                    _logger.LogInformation($"Record is not found.");

                    return NotFound();
                }

                _logger.LogInformation($"Record updated successfully.");

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(nameof(UsersController), ex.Message, ex.StackTrace);

                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPatch("changeStatus/user/{id:guid}")]
        public async Task<ActionResult> Patch(Guid id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var result = await _service.ChangeStatus(id);

                if (_notifier.HasNotification())
                {
                    return BadRequest(Responses.GetErrors(_notifier.GetNotification()));
                }

                if (result == null)
                {
                    _logger.LogInformation($"Record is not found.");

                    return NotFound();
                }

                _logger.LogInformation($"Record updated successfully.");

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(nameof(UsersController), ex.Message, ex.StackTrace);

                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var result = await _service.Delete(id);

                if (_notifier.HasNotification())
                {
                    return BadRequest(Responses.GetErrors(_notifier.GetNotification()));
                }

                if (result) return Ok(true);

                _logger.LogInformation($"Record is not found.");

                return NotFound();
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(nameof(UsersController), ex.Message, ex.StackTrace);

                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var result = await _service.GetAll();

                return result == null ? NotFound() : Ok(result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(nameof(UsersController), ex.Message, ex.StackTrace);

                return StatusCode((int) HttpStatusCode.InternalServerError, ex.Message);
            } 
        }

        [HttpGet("{id:guid}", Name = "GetWithId")]
        public async Task<ActionResult> Get(Guid id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var result = await _service.Get(id);

                if (result != null) return Ok(result);
                
                _logger.LogInformation($"Record is not found.");
                    
                return NotFound();
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(nameof(UsersController), ex.Message, ex.StackTrace);

                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
