using Biugra.Domain.Models;
using Biugra.Domain.Models.Dtos.Category;
using Biugra.Domain.Models.Dtos.Forum;
using Biugra.Infrastructure.Interfaces.Repositories;
using Google;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Biugra.Domain.Interfaces;
using Biugra.Service.Utilities.Helpers;

namespace Biugra.Web.ViewComponents;

[ViewComponent(Name = "Category")]
public class ViewComponents : ViewComponent
{
    private readonly HttpClient _client;
    private readonly ICurrentUserService _currentUserService;
    public ViewComponents(HttpClient client, ICurrentUserService currentUserService)
    {
        _client = client;
        var token = $"Bearer {currentUserService.GetToken()}";
        _currentUserService = currentUserService;
        _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {currentUserService.GetToken()}");
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var response = await _client.GetAsync("Category/GetCategories");
        var data = await response.ReadContentAs<List<CategoryDTO>>();
        return View("Index", data.Data);
    }
}

