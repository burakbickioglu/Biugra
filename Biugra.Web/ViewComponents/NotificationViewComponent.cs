using Biugra.Domain.Models.Dtos.Notification;
using Microsoft.AspNetCore.Mvc;
using Biugra.Domain.Interfaces;
using Biugra.Service.Utilities.Helpers;

namespace Biugra.Web.ViewComponents;

[ViewComponent(Name = "Notification")]
public class NotificationViewComponent : ViewComponent
{
    private readonly HttpClient _client;
    private readonly ICurrentUserService _currentUserService;

    public NotificationViewComponent(HttpClient client, ICurrentUserService currentUserService)
    {
        _client = client;
        var token = $"Bearer {currentUserService.GetToken()}";
        _currentUserService = currentUserService;
        _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {currentUserService.GetToken()}");
    }
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var userId = _currentUserService.GetUserId();
        var response = await _client.GetAsync($"Profile/GetUserNotifications/{userId}");
        var data = await response.ReadContentAs<List<NotificationDTO>>();
        return View("Index", data.Data);
    }
}

