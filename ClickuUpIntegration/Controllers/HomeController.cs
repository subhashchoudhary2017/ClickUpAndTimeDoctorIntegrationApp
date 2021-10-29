using ClickUpIntegration.Models;
using ClickuUpIntegration.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ClickuUpIntegration.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpAccessor;
        public HomeController(ILogger<HomeController> logger,
            IWebHostEnvironment env,
            IHttpContextAccessor httpAccessor)
        {
            _logger = logger;
            _env = env;
            _httpAccessor = httpAccessor;
        }
        public IActionResult Login()
        {
            return View();
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

        public IActionResult RefreshToken()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SaveToken(Token data)
        {
            try
            {
                var filePath = _env.ContentRootPath + "/refreshtoken.json";
                // Read existing json data
                var jsonData = System.IO.File.ReadAllText(filePath);
                // De-serialize to object or create new list
                var tokenReadDto = JsonConvert.DeserializeObject<TokenReadDto>(jsonData)
                                      ?? new TokenReadDto();

                var newTokenDto = JsonConvert.DeserializeObject<TokenReadDto>(data.Result)
                                      ?? new TokenReadDto();


                tokenReadDto = newTokenDto;

                // Update json data string
                jsonData = JsonConvert.SerializeObject(tokenReadDto);
                System.IO.File.WriteAllText(filePath, jsonData);

                _httpAccessor.HttpContext.Response.Cookies.Append("timedoctor_accesstoken", tokenReadDto.Data.Token, new CookieOptions
                {
                    Expires = DateTime.Parse(tokenReadDto.Data.ExpiresAt)
                });

                return Json(new { Success = true, Message = "" });
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message });
            }

        }
    }
}
