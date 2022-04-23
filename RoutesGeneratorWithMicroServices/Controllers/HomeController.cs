using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RoutesGeneratorWithMicroServices.Models;

namespace RoutesGeneratorWithMicroServices.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _hostEnvironment;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment hostEnvironment)
        {
            _logger = logger;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
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
                string fileName = Path.GetFileNameWithoutExtension(fileReceived.File.FileName);
                string extension = Path.GetExtension(fileReceived.File.FileName);
                fileReceived.FileName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/file", fileName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await fileReceived.File.CopyToAsync(fileStream);
                }

                return RedirectToAction(nameof(Index));
            }
            return View(fileReceived);
        }
    }
}
