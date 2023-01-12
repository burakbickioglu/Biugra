using Biugra.Domain.Models.Dtos.Category;
using Biugra.Domain.Models.Dtos.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Biugra.Domain.Interfaces;
using Biugra.Service.Utilities.Helpers;

namespace Biugra.BackOffice.Controllers
{
    [Authorize/*(Roles = "SuperAdmin")*/]
    public class CategoryController : Controller
    {
        private readonly HttpClient _client;
        private readonly ICurrentUserService _currentUserService;

        public CategoryController(HttpClient client, ICurrentUserService currentUserService)
        {
            _client = client;
            var token = $"Bearer {currentUserService.GetToken()}";
            _currentUserService = currentUserService;
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {currentUserService.GetToken()}");
        }

        public async Task<IActionResult> Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(CategoryDTO category)
        {
            if (ModelState.IsValid)
            {
                var response = await _client.PostAsJsonAsync($"Admin/AddCategory", category);
                var data = await response.ReadContentAs<CategoryDTO>();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        public async Task<IActionResult> Index()
        {
            var response = await _client.GetAsync("Category/GetCategories");
            var data = await response.ReadContentAs<List<CategoryDTO>>();
            return View(data.Data);
        }

        public async Task<IActionResult> Update(Guid id)
        {
            var response = await _client.GetAsync($"Category/GetCategory/{id}");
            var categoryData = await response.ReadContentAs<CategoryDTO>();
            return View(categoryData.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Update(CategoryDTO model)
        {
            if (ModelState.IsValid)
            {
                var response = await _client.PostAsJsonAsync($"Admin/UpdateCategory", model);
                var data = await response.ReadContentAs<CategoryDTO>();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Remove(Guid id)
        {
            var response = await _client.DeleteAsync($"Admin/DeleteCategory/{id}");
            return RedirectToAction(nameof(Index));
        }
    }
}
