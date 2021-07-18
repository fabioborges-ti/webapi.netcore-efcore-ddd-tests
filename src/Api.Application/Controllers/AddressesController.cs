using Api.Domain.Dtos.Address;
using Api.Domain.Interfaces.Services.Address;
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
    public class AddressesController : ControllerBase
    {
        private readonly ILogger<AddressesController> _logger;
        private readonly IAddressService _service;

        public AddressesController(ILogger<AddressesController> logger, IAddressService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] AddressDtoCreate address)
        {
            if (!ModelState.IsValid) return BadRequest();

            try
            {
                var result = await _service.Post(address);

                return result != null
                    ? Created(new Uri(Url.Link("GetAddressWithId", new {id = result.Id}) ?? string.Empty), result) 
                    : BadRequest();
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(nameof(AddressesController), ex.Message, ex.StackTrace);

                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> Put([FromBody] AddressDtoUpdate address)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var result = await _service.Put(address);

                return result != null ? Ok(result) : NotFound();
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(nameof(AddressesController), ex.Message, ex.StackTrace);

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

                return result ? Ok() : BadRequest();
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(nameof(AddressesController), ex.Message, ex.StackTrace);

                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet("address/{id:guid}", Name = "GetAddressWithId")]
        public async Task<ActionResult> GetById(Guid id)
        {
            if (!ModelState.IsValid) return BadRequest();

            try
            {
                var result = await _service.Get(id);

                return result == null ? NotFound() : Ok(result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(nameof(AddressesController), ex.Message, ex.StackTrace);

                return StatusCode((int) HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet("user/{id:guid}")]
        public async Task<ActionResult> GetByIdUser(Guid id)
        {
            if (!ModelState.IsValid) return BadRequest();

            try
            {
                var result = await _service.GetByUser(id);

                return result == null ? NotFound() : Ok(result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(nameof(AddressesController), ex.Message, ex.StackTrace);

                return StatusCode((int) HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
