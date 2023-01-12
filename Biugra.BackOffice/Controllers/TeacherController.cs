using Biugra.Domain.Enums;
using Biugra.Domain.Models.Dtos.Category;
using Biugra.Domain.Models.Dtos.Teacher;
using Microsoft.AspNetCore.Mvc;
using Biugra.Domain.Interfaces;
using Biugra.Service.Utilities.Helpers;

namespace Biugra.BackOffice.Controllers
{
    public class TeacherController : Controller
    {
        private readonly HttpClient _client;
        private readonly ICurrentUserService _currentUserService;

        public TeacherController(HttpClient client, ICurrentUserService currentUserService)
        {
            _client = client;
            var token = $"Bearer {currentUserService.GetToken()}";
            _currentUserService = currentUserService;
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {currentUserService.GetToken()}");
        }
        public async Task<IActionResult> Index()
        {
            var response = await _client.GetAsync("Teacher/GetTeachers");
            var data = await response.ReadContentAs<List<TeacherDTO>>();
            return View(data.Data);
        }

        public async Task<IActionResult> Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(TeacherDTO teacher)
        {
            if (ModelState.IsValid)
            {
                var response = await _client.PostAsJsonAsync($"Admin/AddTeacher", teacher);
                var data = await response.ReadContentAs<TeacherDTO>();
                return RedirectToAction(nameof(Index));
            }
            return View(teacher);
        }

        public async Task<IActionResult> Update(Guid id)
        {
            var response = await _client.GetAsync($"Teacher/GetTeacher/{id}");
            var teacherData = await response.ReadContentAs<TeacherDTO>();
            return View(teacherData.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Update(TeacherDTO model)
        {
            if (ModelState.IsValid)
            {
                var response = await _client.PostAsJsonAsync($"Admin/UpdateTeacher", model);
                var data = await response.ReadContentAs<TeacherDTO>();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Remove(Guid id)
        {
            var response = await _client.DeleteAsync($"Admin/DeleteTeacher/{id}");
            return RedirectToAction(nameof(Index));
        }
    }
}
