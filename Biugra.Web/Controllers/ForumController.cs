using Biugra.Domain.Models.Dtos.Category;
using Biugra.Domain.Models.Dtos.Comment;
using Biugra.Domain.Models.Dtos.Forum;
using Biugra.Domain.Models.Dtos.ProvidedService;
using Biugra.Domain.Models.Dtos.Teacher;
using Biugra.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Biugra.Domain.Interfaces;
using Biugra.Service.Utilities.Helpers;

namespace Biugra.Web.Controllers;
[Authorize(Roles = "Admin, SystemUser")]
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
    public async Task<IActionResult> Index(string? category, string? message)
    {
        if (!string.IsNullOrEmpty(message))
            ViewBag.message = message;
        ViewBag.Title = !string.IsNullOrEmpty(category) ? category : "Forumlar";

        var requestUrl = "Forum/GetAllForums";
        if (!string.IsNullOrEmpty(category))
        {
            requestUrl = $"Forum/GetCategoryForums/{category}";
        }
        var response = await _client.GetAsync(requestUrl);

        var data = await response.ReadContentAs<List<ForumResponseDTO>>();
        return View(data.Data != null ? data.Data : new List<ForumResponseDTO>());
    }

    public async Task<IActionResult> LikedForums()
    {
        var response = await _client.GetAsync($"Forum/GetLikedForums");
        var data = await response.ReadContentAs<List<ForumResponseDTO>>();
        return View(data.Data != null ? data.Data : new List<ForumResponseDTO>());
    }


    public async Task<IActionResult> Add()
    {
        var categories = await _client.GetAsync($"Category/GetCategories");
        var categoriesData = await categories.ReadContentAs<List<CategoryDTO>>();
        return View(new AddForumViewModel { Categories = categoriesData.Data });
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddForumViewModel model)
    {

        var forum = model.Forum;
        if (ModelState.IsValid)
        {
            var response = await _client.PostAsJsonAsync($"Forum/AddForum", forum);
            var data = await response.ReadContentAs<AddForumResponseDTO>();
            return RedirectToAction(nameof(Index), new RouteValueDictionary(new { Controller = "Forum", Action = "Index", message = "Forum başarıyla oluşturuldu !" }));
        }
        var categories = await _client.GetAsync($"Category/GetCategories");
        var categoriesData = await categories.ReadContentAs<List<CategoryDTO>>();
        return View(new AddForumViewModel { Categories = categoriesData.Data, Forum = model.Forum });

    }

    public async Task<IActionResult> Detail(Guid id, string? message)
    {
        if (!string.IsNullOrEmpty(message))
            ViewBag.message = message;

        var response = await _client.GetAsync($"Forum/GetForum/{id}");
        var data = await response.ReadContentAs<ForumResponseDTO>();
        data.Data.Comments = data.Data.Comments.OrderByDescending(x => x.CreatedOn).ToList();
        return View(new AddCommentViewModel { Forum = data.Data });
    }

    [HttpPost]
    public async Task<IActionResult> AddComment(AddCommentViewModel model)
    {
        var comment = model.NewComment;
        if (ModelState.IsValid)
        {
            var response = await _client.PostAsJsonAsync($"Forum/AddComment", comment);
            var data = await response.ReadContentAs<AddCommentResponseDTO>();
            return RedirectToAction("Detail", new RouteValueDictionary(new { Controller = "Forum", Action = "Detail", Id = data.Data.ForumId, message = "Yorum başarıyla oluşturuldu !" }));
        }
        return RedirectToAction("Detail", new RouteValueDictionary(new { Controller = "Forum", Action = "Detail", Id = model.NewComment.ForumId }));
    }

}
