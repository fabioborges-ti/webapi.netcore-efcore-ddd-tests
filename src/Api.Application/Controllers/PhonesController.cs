using Api.Domain.Dtos.Phone;
using Api.Domain.Interfaces.Services.Phone;
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
    public class PhonesController : ControllerBase
    {
        private readonly ILogger<PhonesController> _logger;
        private readonly IPhoneService _service;

        public PhonesController(ILogger<PhonesController> logger, IPhoneService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] PhoneDtoCreate phone)
        {
            if (!ModelState.IsValid) return BadRequest();

            try
            {
                var result = await _service.Post(phone);

                return result != null
                    ? Created(new Uri(Url.Link("GetPhoneWithId", new {id = result.Id}) ?? string.Empty), result)
                    : BadRequest();
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(nameof(PhonesController), ex.Message, ex.StackTrace);

                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> Put([FromBody] PhoneDtoUpdate phone)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var result = await _service.Put(phone);

                return result != null ? Ok(result) : NotFound();
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(nameof(PhonesController), ex.Message, ex.StackTrace);

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

                return (result) ? Ok() : BadRequest();
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(nameof(PhonesController), ex.Message, ex.StackTrace);

                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet("phone/{id:guid}", Name = "GetPhoneWithId")]
        public async Task<ActionResult> GetById(Guid id)
        {
            if (!ModelState.IsValid) return BadRequest();

            try
            {
                var result = await _service.Get(id);

                return result != null ? Ok(result) : NotFound();
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(nameof(PhonesController), ex.Message, ex.StackTrace);

                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet("user/{id:guid}")]
        public async Task<ActionResult> GetByIdUser(Guid id)
        {
            if (!ModelState.IsValid) return BadRequest();

            try
            {
                var result = await _service.GetByUser(id);

                return result != null ? Ok(result) : NotFound();
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(nameof(PhonesController), ex.Message, ex.StackTrace);

                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
