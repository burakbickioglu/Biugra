using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Biugra.Domain.Models.Dtos.Authorization;
using Biugra.Service.Utilities.Helpers;
using System.Security.Claims;
using Biugra.Service.Utilities;
using Biugra.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc.Localization;

namespace Biugra.BackOffice.Controllers;

public class AccountController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly JwtSettings _settings;
    private readonly ICurrentUserService _currentUserService;
    private readonly IHtmlLocalizer<AccountController> _localizer;

    public AccountController(HttpClient httpClient, JwtSettings settings, ICurrentUserService currentUserService, IHtmlLocalizer<AccountController> localizer)
    {
        _httpClient = httpClient;
        _settings = settings;
        _currentUserService = currentUserService;
        _localizer = localizer;
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginRequestDTO model)
    {
        var response = await _httpClient.PostAsJsonAsync("auth/login", model);
        if (!response.IsSuccessStatusCode)
            return View();
        var responseBody = await response.ReadContentAs<AuthResponseDTO>();

        if (!ModelState.IsValid)
            return View(model);


        if (responseBody.IsSucceed && responseBody?.Data?.Token is not null)
        {

            var claimsPrincipal = JwtHelper.GetPrincipalFromToken(_settings, responseBody.Data.Token);
            var claims = claimsPrincipal.Claims.ToList();

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1),
                IssuedUtc = DateTimeOffset.UtcNow,
                IsPersistent = true
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return RedirectToAction("Index", "Home");
        }


        ModelState.AddModelError("error", responseBody?.Message ?? string.Empty);
        return View(model);
    }

    public IActionResult Logout()
    {
        _currentUserService.Logout();
        foreach (var cookie in Request.Cookies.Keys)
        {
            Response.Cookies.Delete(cookie);
        }
        return RedirectToAction(nameof(Login));
    }
}
