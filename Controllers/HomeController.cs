using GoodFoodProjectMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Data.SqlClient;

namespace GoodFoodProjectMVC.Controllers
{
    public class HomeController : Controller
    {
        private const string ConnectionString = "Server=localhost\\SQLEXPRESS;Database=goodfood;Integrated Security=True;TrustServerCertificate=True;";

        [HttpPost]
        public IActionResult AddRecipe(Recipes recipe)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO Recipes (Name, Description) VALUES (@Name, @Description)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", recipe.Name);
                        command.Parameters.AddWithValue("@Description", recipe.Description);

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
            List<Recipes> recipes = new List<Recipes>();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT Name, Description FROM Recipes";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            recipes.Add(new Recipes
                            {
                                Name = reader.GetString(0),
                                Description = reader.GetString(1)
                            });
                        }
                    }
                }
            }
            return View(recipes);
        }

        // GET: /Home/Delete/5
        public IActionResult Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT Id, Name, Description FROM Recipes WHERE Id = @Id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Recipes recipe = new Recipes
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Description = reader.GetString(2)
                            };
                            // Zwróć widok do potwierdzenia usunięcia rekordu
                            return View(recipe);
                        }
                    }
                }
            }
            return NotFound();
        }

        // POST: /Home/Delete/5
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "DELETE FROM Recipes WHERE Id = @Id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        // Rekord został pomyślnie usunięty, możesz zdecydować, co dalej zrobić
                        // Możesz przekierować użytkownika na inną stronę lub zaktualizować bieżącą stronę
                        // W tym przykładzie przekierowujemy użytkownika na stronę z listą przepisów
                        return RedirectToAction("Recipes");
                    }
                }
            }
            return NotFound();
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
        public IActionResult DataBase()
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

