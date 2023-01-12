using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Biugra.Application.Services.Authentication.Commands;
using Biugra.Domain.Models.Dtos.Authorization;
using Biugra.Domain.Models.ViewModels;
using Biugra.Service.Utilities.Helpers;
using System.Security.Claims;
using Biugra.Service.Utilities;
using Biugra.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Biugra.Domain.Models.Dtos.Category;
using LanguageExt.Pipes;
using Biugra.Domain.Models.Dtos.Forum;
using Biugra.Domain.Models.Dtos.ProvidedService;
using Biugra.Domain.Models.Dtos.User;
using Biugra.Domain.Models.Dtos.Wallet;
using Biugra.Web.Models;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Biugra.Domain.Models.Dtos.Message;

namespace Biugra.BackOffice.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient _client;
        private readonly JwtSettings _settings;
        private readonly ICurrentUserService _currentUserService;
        public AccountController(HttpClient httpClient, JwtSettings settings, ICurrentUserService currentUserService)
        {
            _client = httpClient;
            _settings = settings;
            _currentUserService = currentUserService;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequestDTO model)
        {

            if (!ModelState.IsValid)
                return View(model);
            var response = await _client.PostAsJsonAsync("auth/register", model);
            if (!response.IsSuccessStatusCode)
                return View();
            var responseBody = await response.ReadContentAs<AuthResponseDTO>();

            if (responseBody.IsSucceed)
            {
                return RedirectToAction(nameof(Login), new RouteValueDictionary(new { Controller = "Account", Action = "Login", model = new LoginRequestDTO { Email = model.Email, Password = model.Password } }));
            }
            ModelState.AddModelError("error", responseBody?.Message ?? string.Empty);
            return View(model);
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDTO model)
        {

            if (!ModelState.IsValid)
                return View(model);

            var response = await _client.PostAsJsonAsync("auth/login", model);
            if (!response.IsSuccessStatusCode)
                return View();
            var responseBody = await response.ReadContentAs<AuthResponseDTO>();



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

                return RedirectToAction("Index", "Forum");
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

        public async Task<IActionResult> Profile(string? message)
        {
            if (!string.IsNullOrEmpty(message))
                ViewBag.message = message;

            var userId = _currentUserService.GetUserId();
            var user = await _client.GetAsync($"Profile/GetUser/{userId}");
            var userData = await user.ReadContentAs<UserDTO>();

            var forums = await _client.GetAsync($"Forum/GetUserForums/{userId}");
            var forumDatas = await forums.ReadContentAs<List<ForumResponseDTO>>();

            var providedServices = await _client.GetAsync($"ProvidedService/GetUserProvidedServices/{userId}");
            var providedServicesDatas = await providedServices.ReadContentAs<List<UserProvidedServiceDTO>>();

            var wallet = await _client.GetAsync($"Profile/GetWallet/{userId}");
            var walletData = await wallet.ReadContentAs<WalletDTO>();

            return View(new ProfileViewModel { Forums = forumDatas.Data, User = userData.Data, ProvidedServices = providedServicesDatas.Data, Wallet = walletData.Data });

        }

        public async Task<IActionResult> EditProfile()
        {
            var userId = _currentUserService.GetUserId();
            var user = await _client.GetAsync($"Profile/GetUser/{userId}");
            var userData = await user.ReadContentAs<UserDTO>();

            return View(userData.Data);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(UserDTO model)
        {
            if (ModelState.IsValid)
            {
                var response = await _client.PostAsJsonAsync($"Profile/EditProfile", model);
                var data = await response.ReadContentAs<UserDTO>();
                return RedirectToAction(nameof(Profile));
            }
            return View(model);
        }

        public async Task<IActionResult> ContactUs()
        {
            return View(new MessageDTO());
        }

        [HttpPost]
        public async Task<IActionResult> ContactUs(MessageDTO model)
        {
            if (ModelState.IsValid)
            {
                model.AppUserId = _currentUserService.GetUserId();
                var response = await _client.PostAsJsonAsync($"Forum/SendMessage", model);
                var data = await response.ReadContentAs<MessageDTO>();
                return RedirectToAction("Profile", "Account", new RouteValueDictionary(new { Controller = "Profile", Action = "Account", message = "Mesajınız iletilmiştir. En kısa sürede sisteme kayıtlı mail adresiniz üzerinden dönüş sağlanacaktır." }));
            }
            return View(model);
        }

    }
}
