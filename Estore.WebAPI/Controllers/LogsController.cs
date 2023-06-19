using Estore.Core.Entities;
using Estore.Service.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Estore.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly IService<Log> _service;

        public LogsController(IService<Log> service)
        {
            _service = service;
        }
        // GET: api/<LogsController>
        [HttpGet]
        public async Task<IEnumerable<Log>> GetAsync()
        {
            return await _service.GetAllAsync();
        }

        // GET api/<LogsController>/5
        [HttpGet("{id}")]
        public async Task<Log> GetAsync(int id)
        {
            return await _service.FindAsync(id);
        }

        // POST api/<LogsController>
        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody] Log value)
        {
            await _service.AddAsync(value);
            await _service.SaveAsync();
            return Ok(value);
        }

        // PUT api/<LogsController>/5
        [HttpPut]
        public async Task<ActionResult> Put([FromBody] Log value)
        {
            _service.Update(value);
            await _service.SaveAsync();
            return Ok(value);
        }

        // DELETE api/<LogsController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            _service.Delete(await _service.FindAsync(id));
            await _service.SaveAsync();
            return Ok();
        }
    }
}
