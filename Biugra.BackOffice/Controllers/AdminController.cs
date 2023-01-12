using Biugra.BackOffice.Models;
using Biugra.Domain.Models.Dtos.Forum;
using Biugra.Domain.Models.Dtos.Message;
using Biugra.Domain.Models.Dtos.ProvidedService;
using Biugra.Domain.Models.Dtos.User;
using Biugra.Domain.Models.Dtos.Wallet;
using LanguageExt.Pipes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Biugra.Domain.Interfaces;
using Biugra.Service.Services;
using Biugra.Service.Utilities.Helpers;
using System.Globalization;

namespace Biugra.BackOffice.Controllers
{
    [Authorize/*(Roles = "SuperAdmin")*/]
    public class AdminController : Controller
    {
        private readonly HttpClient _client;
        private readonly ICurrentUserService _currentUserService;

        public AdminController(HttpClient httpClient, ICurrentUserService currentUserService)
        {
            _client = httpClient;
            var token = $"Bearer {currentUserService.GetToken()}";
            _currentUserService = currentUserService;
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {currentUserService.GetToken()}");
        }

        public async Task<IActionResult> UserList(string? filter)
        {
            var response = await _client.GetAsync("Admin/GetUsers");
            var data = await response.ReadContentAs<List<UserDTO>>();
            if (!string.IsNullOrEmpty(filter))
            {
                filter = filter.ToLower();

                return View(data.Data.Where(p => (!string.IsNullOrEmpty(p.FirstName) && p.FirstName.ToLower().Contains(filter))
                || (!string.IsNullOrEmpty(p.LastName) && p.LastName.ToLower().Contains(filter))
                || (!string.IsNullOrEmpty(p.Email) && p.Email.ToLower().Contains(filter))).ToList());
            }

            return View(data.Data);
        }

        //[HttpDelete("{id}")]
        public async Task<IActionResult> Remove(Guid id)
        {
            var response = await _client.DeleteAsync($"Admin/DeleteUser/{id}");
            return RedirectToAction(nameof(UserList));
        }


        public async Task<IActionResult> UserDetail(Guid id)
        {
            var user = await _client.GetAsync($"Profile/GetUser/{id}");
            var userData = await user.ReadContentAs<UserDTO>();

            var forums = await _client.GetAsync($"Forum/GetUserForums/{id}");
            var forumDatas = await forums.ReadContentAs<List<ForumResponseDTO>>();

            var providedServices = await _client.GetAsync($"ProvidedService/GetUserProvidedServices/{id}");
            var providedServicesDatas = await providedServices.ReadContentAs<List<UserProvidedServiceDTO>>();

            var wallet = await _client.GetAsync($"Profile/GetWallet/{id}");
            var walletData = await wallet.ReadContentAs<WalletDTO>();

            return View(new UserDetailViewModel { Forums = forumDatas.Data, User = userData.Data, ProvidedServices = providedServicesDatas.Data, Wallet = walletData.Data });

        }
        public async Task<IActionResult> Update(Guid id)
        {
            var userResponse = await _client.GetAsync($"Admin/GetUser/{id}");
            var userData = await userResponse.ReadContentAs<UserDTO>();
            ViewBag.CountryList = GetCountries();

            return View(userData.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UserDTO model)
        {
            if (ModelState.IsValid)
            {
                var response = await _client.PostAsJsonAsync($"Admin/UpdateUser", model);
                var data = await response.ReadContentAs<UserDTO>();
                return RedirectToAction(nameof(UserList));
            }
            return View(model);
        }

        public async Task<IActionResult> Add()
        {
            ViewBag.CountryList = GetCountries();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddUserDTO user)
        {
            if (ModelState.IsValid)
            {
                var response = await _client.PostAsJsonAsync($"Admin/AddUser", user);
                var data = await response.ReadContentAs<UserDTO>();
                return RedirectToAction(nameof(UserList));
            }
            return View(user);
        }

        public async Task<IActionResult> DeleteTest(Guid userId, Guid testId)
        {
            var response = await _client.DeleteAsync($"Test/DeleteTest/{testId}");
            return RedirectToAction(nameof(Update), new { id = userId });
        }

        public async Task<IActionResult> Messages()
        {
            var response = await _client.GetAsync("Admin/GetMessages");
            var messages = await response.ReadContentAs<List<MessageDTO>>();

            return View(messages.Data);
        }

        public List<string> GetCountries()
        {
            List<string> countryList = new List<string>();
            CultureInfo[] CInfoList = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            foreach (CultureInfo CInfo in CInfoList)
            {
                RegionInfo R = new RegionInfo(CInfo.LCID);
                if (!(countryList.Contains(R.EnglishName)))
                {
                    countryList.Add(R.EnglishName);
                }
            }

            countryList.Sort();
            return countryList;
        }



    }
}
