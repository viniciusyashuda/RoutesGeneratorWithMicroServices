
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RoutesGeneratorWithMicroServices.Models;
using RoutesGeneratorWithMicroServices.Services;

namespace RoutesGeneratorWithMicroServices.Controllers
{
    //[Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _hostEnvironment;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment hostEnvironment)
        {
            _logger = logger;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            string user = "Anonymous";
            bool authenticate = false;

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                user = HttpContext.User.Identity.Name;
                authenticate = true;

                if (HttpContext.User.IsInRole("Admin"))
                    ViewBag.Role = "Admin";
                else
                    ViewBag.Role = "User";
            }
            else
            {
                user = "Not logged!";
                authenticate = false;
                ViewBag.Role = "";
            }

            var users = await UserQueries.GetAllUsers();
            if (users.Count < 1)
            {
                UserQueries.PostUser(new User() { Name = "Admin", Login = "Admin", Password = "Admin", Role = "Admin" });
                ViewBag.User = "Admin";
                ViewBag.Role = "Admin";
                ViewBag.Authenticate = true;
                return RedirectToRoute(new { controller = "Users", action = "Index" });
            }
            ViewBag.User = user;
            ViewBag.Authenticate = authenticate;
            return View();

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        // GET: Files/Create
        public IActionResult ReceiveFile()
        {
            string user = "Anonymous";
            bool authenticate = false;

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                user = HttpContext.User.Identity.Name;
                authenticate = true;

                if (HttpContext.User.IsInRole("Admin"))
                    ViewBag.Role = "Admin";
                else
                    ViewBag.Role = "User";
            }
            else
            {
                user = "Not logged!";
                authenticate = false;
                ViewBag.Role = "";
            }

            ViewBag.User = user;
            ViewBag.Authenticate = authenticate;
            return View();
        }

        // POST: Files/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReceiveFile([Bind("Id,FileName, File")] FileReceiver fileReceived)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = "Plan";
                string extension = Path.GetExtension(fileReceived.File.FileName);
                fileReceived.FileName = fileName = fileName + extension;
                string path = Path.Combine(wwwRootPath + "/file", fileName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await fileReceived.File.CopyToAsync(fileStream);
                }

                ReadFile.OrderFile(path);

                return RedirectToAction(nameof(Index));
            }
            return View(fileReceived);
        }
    }
}
