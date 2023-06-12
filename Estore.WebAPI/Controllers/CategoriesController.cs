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
        private readonly ICategoryService _service;

        public CategoriesController(ICategoryService service)
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
            return await _service.GetCategoryByIncludeAsync(id);
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
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var kayit = _service.Find(id);
            if (kayit == null)
            {
                return NotFound();
            }
            _service.Delete(kayit);
            _service.Save();
            return Ok(kayit);
        }
    }
}
