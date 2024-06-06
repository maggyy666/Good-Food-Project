using GoodFoodProjectMVC.Models;
using GoodFoodProjectMVC.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace GoodFoodProjectMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly MongoDBService _mongoDBService;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public HomeController(MongoDBService mongoDBService, IWebHostEnvironment hostingEnvironment)
        {
            _mongoDBService = mongoDBService;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginUser(string username, string password)
        {
            var user = await _mongoDBService.GetUserByUsernameAndPasswordAsync(username, password);
            if (user != null)
            {
                HttpContext.Session.SetString("Username", user.Username);
                ViewBag.Message = "Logged in as " + user.Username;
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "Invalid username or password";
                return View("Login");
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser(string username, string password)
        {
            var user = new User { Username = username, Password = password };
            try
            {
                await _mongoDBService.InsertUserAsync(user);
                ViewBag.Message = "User registered: " + username;
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error: " + ex.Message;
            }
            return View("Index");
        }

        public IActionResult AddRecipe()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Recipes()
        {
            List<FormData> recipes;
            try
            {
                recipes = await _mongoDBService.GetAllRecipesAsync();
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error: " + ex.Message;
                recipes = new List<FormData>();
            }
            return View(recipes);
        }

        public async Task<IActionResult> Recipe(string id)
        {
            var recipe = await _mongoDBService.GetRecipeByIdAsync(id);
            if (recipe == null)
            {
                return NotFound($"Recipe with id {id} not found.");
            }
            return View("BaseRecipe", recipe);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitData(string title, string description, IFormFile imageFile)
        {
            string imageBase64 = null;
            if (imageFile != null && imageFile.Length > 0)
            {
                // Save the image to the "images" folder in wwwroot
                var fileName = Path.GetFileName(imageFile.FileName);
                var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "images", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                // Encode the image as Base64
                using (var memoryStream = new MemoryStream())
                {
                    await imageFile.CopyToAsync(memoryStream);
                    var imageBytes = memoryStream.ToArray();
                    imageBase64 = Convert.ToBase64String(imageBytes);
                }
            }

            var formData = new FormData { Title = title, Description = description, ImageBase64 = imageBase64 };
            try
            {
                await _mongoDBService.InsertDataAsync(formData);
                ViewBag.Message = "Data saved to MongoDB: " + title + ", Description: " + description;
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error: " + ex.Message;
            }
            return View("Index");
        }
    }
}
