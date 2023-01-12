using Biugra.BackOffice.Models;
using Biugra.Domain.Models.Dtos.Category;
using Biugra.Domain.Models.Dtos.ProvidedService;
using Biugra.Domain.Models.Dtos.Teacher;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Biugra.Domain.Interfaces;
using Biugra.Service.Utilities.Helpers;

namespace Biugra.BackOffice.Controllers
{
    public class ProvidedServiceController : Controller
    {
        private readonly HttpClient _client;
        private readonly ICurrentUserService _currentUserService;

        public ProvidedServiceController(HttpClient client, ICurrentUserService currentUserService)
        {
            _client = client;
            var token = $"Bearer {currentUserService.GetToken()}";
            _currentUserService = currentUserService;
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {currentUserService.GetToken()}");
        }
        public async Task<IActionResult> Index()
        {
            var response = await _client.GetAsync("ProvidedService/GetProvidedServices");
            var data = await response.ReadContentAs<List<ProvidedServiceDTO>>();
            return View(data.Data);
        }

        public async Task<IActionResult> Add()
        {
            var teachers = await _client.GetAsync($"Teacher/GetTeachers");
            var teachersData = await teachers.ReadContentAs<List<TeacherDTO>>();

            return View(new AddProvidedServiceViewModel { Teachers = teachersData.Data });
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddProvidedServiceViewModel model)
        {
            var service = model.ProvidedService;
            if (ModelState.IsValid)
            {
                var response = await _client.PostAsJsonAsync($"Admin/AddProvidedService", service);
                var data = await response.ReadContentAs<ProvidedServiceDTO>();
                return RedirectToAction(nameof(Index));
            }
            var teachers = await _client.GetAsync($"Teacher/GetTeachers");
            var teachersData = await teachers.ReadContentAs<List<TeacherDTO>>();

            return View(new AddProvidedServiceViewModel { ProvidedService = model.ProvidedService, Teachers = teachersData.Data });
        }

        public async Task<IActionResult> Update(Guid id)
        {
            var response = await _client.GetAsync($"ProvidedService/GetProvidedService/{id}");
            var providedServiceData = await response.ReadContentAs<ProvidedServiceDTO>();

            var teachers = await _client.GetAsync($"Teacher/GetTeachers");
            var teachersData = await teachers.ReadContentAs<List<TeacherDTO>>();

            return View(new UpdateProvidedServiceViewModel { ProvidedService = providedServiceData.Data, Teachers = teachersData.Data });
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateProvidedServiceViewModel model)
        {
            var service = model.ProvidedService;
            if (ModelState.IsValid)
            {
                var response = await _client.PostAsJsonAsync($"Admin/UpdateProvidedService", service);
                var data = await response.ReadContentAs<ProvidedServiceDTO>();
                return RedirectToAction(nameof(Index));
            }

            var teachers = await _client.GetAsync($"Teacher/GetTeachers");
            var teachersData = await teachers.ReadContentAs<List<TeacherDTO>>();
            return View(new UpdateProvidedServiceViewModel { ProvidedService = model.ProvidedService, Teachers = teachersData.Data });

        }

    }
}
