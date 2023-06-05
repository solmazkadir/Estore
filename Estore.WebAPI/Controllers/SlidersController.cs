using Estore.Core.Entities;
using Estore.Service.Abstract;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Estore.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SlidersController : ControllerBase
    {
        private readonly IService<Slider> _service;

        public SlidersController(IService<Slider> service)
        {
            _service = service;
        }
        // GET: api/<SlidersController>
        [HttpGet]
        public async Task<IEnumerable<Slider>> GetAsync()
        {
            return await _service.GetAllAsync();
        }

        // GET api/<SlidersController>/5
        [HttpGet("{id}")]
        public async Task<Slider> GetAsync(int id)
        {
            return await _service.FindAsync(id);
        }

        // POST api/<SlidersController>
        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody] Slider value)
        {
            await _service.AddAsync(value);
            await _service.SaveAsync();
            return Ok(value);
        }

        // PUT api/<SlidersController>/5
        [HttpPut]
        public async Task<ActionResult> PutAsync( [FromBody] Slider value)
        {
            _service.Update(value);
            var sonuc = await _service.SaveAsync();
            if (sonuc > 0)
            {
                return Ok(value);
            }
            else
            {
                return Problem();
            }

        }

        // DELETE api/<SlidersController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var data = await _service.FindAsync(id);
            if (data == null)
            {
                return NotFound();
            }
            _service.Delete(data);
            var sonuc = await _service.SaveAsync();
            if (sonuc > 0)
            {
                return Ok(data);
            }

            return Problem();


        }
    }
}
