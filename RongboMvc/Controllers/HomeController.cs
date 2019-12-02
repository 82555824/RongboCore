using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rongbo.Entity;
using Rongbo.Models.RequestModels;
using Rongbo.Service;
using RongboMvc.Models;


namespace RongboMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IBookService _bookService;

        public HomeController(ILogger<HomeController> logger, IBookService bookService)
        {
            _logger = logger;
            _bookService = bookService;
        }

        public async Task<IActionResult> Index()
        {
            //User user = null;
            //var name = user.Name;
            // return Ok(new { id = 1 ,name="111"}); ;
            //var model = await _bookService.Get(1);
            //await _bookService.AddEntity(new Book { Name = "新增的书", Price = 9999, CategoryId = 1 });
            var tt = await _bookService.GetPagerAsync();
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

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            return View(loginRequest);
        }
    }
}
