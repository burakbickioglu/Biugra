using Biugra.Domain.Models.Dtos;
using Biugra.Domain.Models.Dtos.Forum;
using LanguageExt.Pipes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Biugra.BackOffice.Models;
using Biugra.Domain.Interfaces;
using Biugra.Service.Utilities.Helpers;
using System.Diagnostics;

namespace Biugra.BackOffice.Controllers
{
    [Authorize/*(Roles = "SuperAdmin, Admin")*/]
    public class HomeController : Controller
    {
        private readonly HttpClient _client;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, HttpClient client)
        {
            _logger = logger;
            _client = client;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _client.GetAsync("Admin/GetDashboard");
            var data = await response.ReadContentAs<DashboardDTO>();
            return View(data.Data);

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CultureManagement(string culture, string returnUrl)
        {
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)), new CookieOptions { Expires = DateTimeOffset.Now.AddDays(30) });

            return LocalRedirect(returnUrl);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}