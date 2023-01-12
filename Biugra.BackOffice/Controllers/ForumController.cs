using Biugra.Domain.Models.Dtos.Category;
using Biugra.Domain.Models.Dtos.Forum;
using Microsoft.AspNetCore.Mvc;
using Biugra.Domain.Interfaces;
using Biugra.Service.Utilities.Helpers;

namespace Biugra.BackOffice.Controllers;
public class ForumController : Controller
{
    private readonly HttpClient _client;
    private readonly ICurrentUserService _currentUserService;

    public ForumController(HttpClient client, ICurrentUserService currentUserService)
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

        var response = await _client.GetAsync("Forum/GetAllForums");
        var data = await response.ReadContentAs<List<ForumResponseDTO>>();
        return View(data.Data);
    }

    public async Task<IActionResult> ForumDetail(Guid id)
    {
        var response = await _client.GetAsync($"Forum/GetForum/{id}");
        var data = await response.ReadContentAs<ForumResponseDTO>();
        data.Data.Comments = data.Data.Comments.OrderByDescending(x => x.CreatedOn).ToList();
        return View(data.Data);
    }

    public async Task<IActionResult> MarkUsefull(Guid forumId, Guid userId)
    {
        var response = await _client.PostAsJsonAsync($"Forum/MarkUsefull", new MarkUsefullDTO { UserId = userId, ForumId = forumId });
        //var data = await response.ReadContentAs<List<ForumResponseDTO>>();
        var message = "Forum yararlı işaretlenemedi !";
        if (response.IsSuccessStatusCode)
        {
            message = "Forum yararlı işaretlendi !";

        }
        return RedirectToAction(nameof(Index), new RouteValueDictionary(new { message = message }));
    }
}
