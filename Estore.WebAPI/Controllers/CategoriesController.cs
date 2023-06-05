using Estore.Core.Entities;
using Estore.Service.Abstract;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Estore.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IService<Category> _service;

        public CategoriesController(IService<Category> service)
        {
            _service = service;
        }
        // GET: api/<CategoriesController>
        [HttpGet]
        public async Task<IEnumerable<Category>> GetAsync()
        {
            return await _service.GetAllAsync();
        }

        // GET api/<CategoriesController>/5
        [HttpGet("{id}")]
        public async Task<Category> GetAsync(int id)
        {
            return await _service.FindAsync(id);
        }

        // POST api/<CategoriesController>
        [HttpPost]
        public async Task<int> Post([FromBody] Category value) //FromQuery = query string ile
        {
            await _service.AddAsync(value);
            return await _service.SaveAsync();
        }

        // PUT api/<CategoriesController>/5
        [HttpPut]
        public async Task<int> Put( [FromBody] Category value)
        {
            _service.Update(value);
            return await _service.SaveAsync();

        }

        // DELETE api/<CategoriesController>/5
        [HttpDelete]
        public async Task<int> Delete([FromBody] Category value)
        {

            _service.Delete(value);
            return await _service.SaveAsync();
        }
    }
}
