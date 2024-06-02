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

        [HttpPost]
        public async Task<ActionResult> SubmitData(string title, string description, IFormFile imageFile)
        {
            string imageBase64 = null;
            if (imageFile != null && imageFile.Length > 0)
            {
                // Zapisz zdjęcie do folderu "images" w wwwroot
                var fileName = Path.GetFileName(imageFile.FileName);
                var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "images", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                // Zakoduj zdjęcie jako Base64
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
    }
}
