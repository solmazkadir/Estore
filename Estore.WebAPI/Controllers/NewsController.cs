using Estore.Core.Entities;
using Estore.Service.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Estore.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly IService<News> _service;

        public NewsController(IService<News> service)
        {
            _service = service;
        }
        // GET: api/<NewsController>
        [HttpGet]
        public async Task<IEnumerable<News>> GetAsync()
        {
            return await _service.GetAllAsync();
        }

        // GET api/<NewsController>/5
        [HttpGet("{id}")]
        public async Task<News> GetAsync(int id)
        {
            return await _service.FindAsync(id);
        }

        // POST api/<NewsController>
        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody] News value)
        {
            await _service.AddAsync(value);
            await _service.SaveAsync();
            return Ok(value);
        }

        // PUT api/<NewsController>/5
        [HttpPut]
        public async Task<ActionResult> Put([FromBody] News value)
        {
            _service.Update(value);
            await _service.SaveAsync();
            return Ok(value);
        }

        // DELETE api/<NewsController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            _service.Delete(await _service.FindAsync(id));
            await _service.SaveAsync();
            return Ok();
        }
    }
}
