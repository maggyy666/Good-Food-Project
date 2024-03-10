using GoodFoodProjectMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Hosting.Server;

namespace GoodFoodProjectMVC.Controllers
{
    public class HomeController : Controller
    {
        private const string ConnectionString = "Server=localhost\\SQLEXPRESS01;Database=goodfood;Trusted_Connection=True;TrustServerCertificate=True;";


        [HttpPost]
        public IActionResult AddRecipe(Recipe recipe)
        {
            if (ModelState.IsValid)
            {
                using(SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO Recipes (Name, Description, ImagePath) VALUES (@Name, @Description, @ImagePath)";
                    using (SqlCommand command = new SqlCommand(query,connection))
                    {
                        command.Parameters.AddWithValue("@Name", recipe.Name);
                        command.Parameters.AddWithValue("@Description", recipe.Description);
                        command.Parameters.AddWithValue("@ImagePath", recipe.ImagePath);

                        command.ExecuteNonQuery();
                    }
                }
                return RedirectToAction("Recipes");
            }
            return View(recipe);
        }



        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Recipes()
        {
            //Get recipes from database
            return View();
        }
        public IActionResult AddRecipe()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
  

    }
}