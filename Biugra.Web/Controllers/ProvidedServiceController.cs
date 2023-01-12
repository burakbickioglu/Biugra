using Biugra.Domain.Models.Dtos.Forum;
using Biugra.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Biugra.Domain.Enums;
using Biugra.Domain.Interfaces;
using Biugra.Service.Utilities.Helpers;
using Biugra.Domain.Models.Dtos.ProvidedService;

namespace Biugra.Web.Controllers;
[Authorize(Roles = "Admin, SystemUser")]
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
    public async Task<IActionResult> Index(string? message)
    {
        if (!string.IsNullOrEmpty(message))
            ViewBag.message = message;

        var response = await _client.GetAsync("ProvidedService/GetProvidedServices");
        var data = await response.ReadContentAs<List<ProvidedServiceDTO>>();
        return View(data.Data != null ? data.Data : new List<ProvidedServiceDTO>());
    }

    public async Task<IActionResult> TakeService(Guid id)
    {
        var response = await _client.GetAsync($"ProvidedService/AddUserProvidedService/{id}");
        var data = await response.ReadContentAs<UserProvidedServiceDTO>();
        if (data.IsSucceed)
        {
            return RedirectToAction("Profile", "Account", new RouteValueDictionary(new { Controller = "Profile", Action = "Account", message = "Hizmetten yararlanmaya başladınız. Sayfanın alt kısmından kontrol edebilirsiniz." }));
        }
        return RedirectToAction(nameof(Index), new RouteValueDictionary(new { Controller = "ProvidedService", Action = "Index", message = "Malesef bakiyeniz yetersiz. Daha fazla faydalı forum paylaşarak BP kazanabilirsiniz." }));
    }
}
