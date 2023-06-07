using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using Task6.Models;
using Task6.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Task6.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService userService;

        public HomeController(ILogger<HomeController> logger, IUserService userService)
        {
            _logger = logger;
            this.userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(UserModel model)
        {
            if (model is null || model.Name.IsNullOrEmpty()) return View();
            
            var user = await userService.GetByNameAsync(model.Name);

            if (user is null)
            {
                await userService.CreateUser(model.Name);
                user = await userService.GetByNameAsync(model.Name);
            }

            HttpContext.Session.SetString("name", user.Name);
            var messages = userService.GetUserMessages(user);
            HttpContext.Response.Cookies.Append("name", user.Name);

            return View("Messages", messages);
        }

        [HttpPost]
        public async Task<ContentResult> SendMessage(string title, string text, string username)
        {
            var user = await userService.GetByNameAsync(HttpContext.Session.GetString("name"));
            var recipient = await userService.GetByNameAsync(username);
            if (recipient is null)
            {
                await userService.CreateUser(username);
                recipient = await userService.GetByNameAsync(username);
            }

            int id = await userService.CreateMessage(user, recipient, title, text);

            return Content(id.ToString());
        }

        [AllowAnonymous]
        public async Task<IActionResult> Autocomplete(string? substring)
        {
            var users = (await userService.FindByNameAsync(substring)).Select(x=>x.Name).ToList();
            return Json(users);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}