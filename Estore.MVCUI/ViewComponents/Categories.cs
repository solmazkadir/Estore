using Estore.Core.Entities;
using Estore.Service.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Estore.MVCUI.Mvc
{
    public class Categories : ViewComponent
    {
        private readonly IService<Category> _service;

        public Categories(IService<Category> service)
        {
            _service = service;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _service.GetAllAsync());
        }
    }
}
