using Estore.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Estore.WebAPIUsing.Mvc
{
    public class Categories : ViewComponent
    {
        private readonly HttpClient _httpClient;

        public Categories(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _httpClient.GetFromJsonAsync<List<Category>>("https://localhost:7064/api/Categories"));
        }
    }
}
