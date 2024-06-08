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
        public async Task<IActionResult> RegisterUser(string username, string email, string password)
        {
            var user = new User { Username = username, Email = email, Password = password };
            try
            {
                await _mongoDBService.InsertUserAsync(user);
                HttpContext.Session.SetString("Username", user.Username); // Dodaj ustawianie sesji po rejestracji
                ViewBag.Message = "User registered: " + username;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error: " + ex.Message;
                return View("Register");
            }
        }

        public IActionResult AddRecipe()
        {
            var username = HttpContext.Session.GetString("Username");
            ViewBag.Username = string.IsNullOrEmpty(username) ? "Guest" : username;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SubmitData(string id, string title, string description, IFormFile imageFile)
        {
            string imageBase64 = null;

            if (imageFile != null && imageFile.Length > 0)
            {
                // Save the new image to the "images" folder in wwwroot
                var fileName = Path.GetFileName(imageFile.FileName);
                var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "images", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                // Encode the new image as Base64
                using (var memoryStream = new MemoryStream())
                {
                    await imageFile.CopyToAsync(memoryStream);
                    var imageBytes = memoryStream.ToArray();
                    imageBase64 = Convert.ToBase64String(imageBytes);
                }
            }

            if (!string.IsNullOrEmpty(id))
            {
                // Fetch existing recipe
                var existingRecipe = await _mongoDBService.GetRecipeByIdAsync(id);

                if (existingRecipe != null)
                {
                    if (imageBase64 == null)
                    {
                        // Preserve the existing image if a new one is not uploaded
                        imageBase64 = existingRecipe.ImageBase64;
                    }

                    // Update the recipe
                    var updatedRecipe = new FormData
                    {
                        Id = id,
                        Title = title,
                        Description = description,
                        ImageBase64 = imageBase64
                    };

                    await _mongoDBService.UpdateRecipeAsync(updatedRecipe);
                    ViewBag.Message = "Recipe updated: " + title;
                }
                else
                {
                    ViewBag.Message = "Recipe not found.";
                }
            }
            else
            {
                // Insert new recipe
                var newRecipe = new FormData
                {
                    Title = title,
                    Description = description,
                    ImageBase64 = imageBase64
                };

                await _mongoDBService.InsertDataAsync(newRecipe);
                ViewBag.Message = "Recipe added: " + title;
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> EditRecipe(string id)
        {
            var recipe = await _mongoDBService.GetRecipeByIdAsync(id);
            if (recipe == null)
            {
                return NotFound($"Recipe with id {id} not found.");
            }

            var username = HttpContext.Session.GetString("Username");
            ViewBag.Username = string.IsNullOrEmpty(username) ? "Guest" : username;
            return View("AddRecipe", recipe);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRecipe(string id)
        {
            var username = HttpContext.Session.GetString("Username");
            {
                return RedirectToAction("Login");
            }
            try
            {
                await _mongoDBService.DeleteRecipeByIdAsync(id);
                ViewBag.Message = "Recipe deleted.";
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error: " + ex.Message;
            }

            return RedirectToAction("Recipes");
        }

        public IActionResult Index()
        {
            var username = HttpContext.Session.GetString("Username");
            ViewBag.Username = string.IsNullOrEmpty(username) ? "Guest" : username;
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
    }
}
